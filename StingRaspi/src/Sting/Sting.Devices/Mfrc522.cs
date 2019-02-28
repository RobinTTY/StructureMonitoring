using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;

namespace Sting.Devices
{
    public class Mfrc522
    {
        // Registers
        private const byte BitFraming = 0x0D;
        private const byte ComIrq = 0x04;
        private const byte ComIrqEnable = 0x02;
        private const byte Command = 0x01;
        private const byte Control = 0x0C;
        private const byte Error = 0x06;
        private const byte FifoData = 0x09;
        private const byte FifoLevel = 0x0A;
        private const byte Mode = 0x11;
        private const byte RxMode = 0x13;
        private const byte TimerMode = 0x2A;
        private const byte TimerPrescaler = 0x2B;
        private const byte TimerReloadHigh = 0x2C;
        private const byte TimerReloadLow = 0x2D;
        private const byte TxAsk = 0x15;
        private const byte TxControl = 0x14;
        private const byte TxMode = 0x12;
        private const byte Version = 0x37;

        // PiccResponses
        private const ushort AnswerToRequest = 0x0004;
        private const byte SelectAcknowledge = 0x08;
        private const byte Acknowledge = 0x0A;

        // PiccCommands
        private const byte Anticollision1 = 0x93;
        private const byte Anticollision2 = 0x20;
        private const byte AuthenticateKeyA = 0x60;
        private const byte AuthenticateKeyB = 0x61;
        private const byte Halt1 = 0x50;
        private const byte Halt2 = 0x00;
        private const byte Read = 0x30;
        private const byte Request = 0x26;
        private const byte Select1 = 0x93;
        private const byte Select2 = 0x70;
        private const byte Write = 0xA0;

        // PcdCommands
        private const byte Idle = 0x00;
        private const byte MifareAuthenticate = 0x0E;
        private const byte MifareTransceive = 0x0C;

        public class Uid
        {
            public byte Bcc { get; }
            public byte[] Bytes { get; }
            public byte[] FullUid { get; }
            public bool IsValid { get; }

            internal Uid(byte[] uid)
            {
                FullUid = uid;
                Bcc = uid[4];

                Bytes = new byte[4];
                Array.Copy(FullUid, 0, Bytes, 0, 4);

                foreach (var b in Bytes)
                {
                    if (b != 0x00)
                        IsValid = true;
                }
            }

            public sealed override bool Equals(object obj)
            {
                if (!(obj is Uid))
                    return false;

                var uidWrapper = (Uid)obj;

                for (var i = 0; i < 5; i++)
                {
                    if (FullUid[i] != uidWrapper.FullUid[i])
                        return false;
                }

                return true;
            }

            public sealed override int GetHashCode()
            {
                var uid = 0;

                for (var i = 0; i < 4; i++)
                    uid |= Bytes[i] << (i * 8);

                return uid;
            }

            public sealed override string ToString()
            {
                var formatString = "x" + (Bytes.Length * 2);
                return GetHashCode().ToString(formatString);
            }
        }

        private SpiDevice Spi { get; set; }
        private GpioController IoController { get; set; }
        private GpioPin ResetPowerDown { get; set; }

        private const string _spiControllerName = "SPI0";
        private const int _spiChipSelectLine = 0;
        private const int _resetPin = 25;

        public async Task InitIo()
        {

            try
            {
                IoController = GpioController.GetDefault();

                ResetPowerDown = IoController.OpenPin(_resetPin);
                ResetPowerDown.Write(GpioPinValue.High);
                ResetPowerDown.SetDriveMode(GpioPinDriveMode.Output);
            }
            /* If initialization fails, throw an exception */
            catch (Exception ex)
            {
                throw new Exception("GPIO initialization failed", ex);
            }

            try
            {
                var settings = new SpiConnectionSettings(_spiChipSelectLine)
                {
                    ClockFrequency = 1000000,
                    Mode = SpiMode.Mode0
                };

                var spiDeviceSelector = SpiDevice.GetDeviceSelector();
                IReadOnlyList<DeviceInformation> devices = await DeviceInformation.FindAllAsync(spiDeviceSelector);

                Spi = await SpiDevice.FromIdAsync(devices[0].Id, settings);

            }
            /* If initialization fails, display the exception and stop running */
            catch (Exception ex)
            {
                throw new Exception("SPI Initialization Failed", ex);
            }


            Reset();
        }

        private void Reset()
        {
            ResetPowerDown.Write(GpioPinValue.Low);
            Task.Delay(50).Wait();
            ResetPowerDown.Write(GpioPinValue.High);
            Task.Delay(50).Wait();

            // Force 100% ASK modulation
            WriteRegister(TxAsk, 0x40);

            // Set CRC to 0x6363
            WriteRegister(Mode, 0x3D);

            // Enable antenna
            SetRegisterBits(TxControl, 0x03);
        }


        public bool IsTagPresent()
        {
            // Enable short frames
            WriteRegister(BitFraming, 0x07);

            // Transceive the Request command to the tag
            Transceive(false, Request);

            // Disable short frames
            WriteRegister(BitFraming, 0x00);

            // Check if we found a card
            return GetFifoLevel() == 2 && ReadFromFifoShort() == AnswerToRequest;
        }

        public Uid ReadUid()
        {
            // Run the anti-collision loop on the card
            Transceive(false, Anticollision1, Anticollision2);

            // Return tag UID from FIFO
            return new Uid(ReadFromFifo(5));
        }

        public void HaltTag()
        {
            // Transceive the Halt command to the tag
            Transceive(false, Halt1, Halt2);
        }

        public bool SelectTag(Uid uid)
        {
            // Send Select command to tag
            var data = new byte[7];
            data[0] = Select1;
            data[1] = Select2;
            uid.FullUid.CopyTo(data, 2);

            Transceive(true, data);

            return GetFifoLevel() == 1 && ReadFromFifo() == SelectAcknowledge;
        }

        internal byte[] ReadBlock(byte blockNumber, Uid uid, byte[] keyA = null, byte[] keyB = null)
        {
            if (keyA != null)
                Authenticate(AuthenticateKeyA, blockNumber, uid, keyA);
            else if (keyB != null)
                Authenticate(AuthenticateKeyB, blockNumber, uid, keyB);
            else
                return null;

            // Read block
            Transceive(true, Read, blockNumber);

            return ReadFromFifo(16);
        }

        internal bool WriteBlock(byte blockNumber, Uid uid, byte[] data, byte[] keyA = null, byte[] keyB = null)
        {
            if (keyA != null)
                Authenticate(AuthenticateKeyA, blockNumber, uid, keyA);
            else if (keyB != null)
                Authenticate(AuthenticateKeyB, blockNumber, uid, keyB);
            else
                return false;

            // Write block
            Transceive(true, Write, blockNumber);

            if (ReadFromFifo() != Acknowledge)
                return false;

            // Make sure we write only 16 bytes
            var buffer = new byte[16];
            data.CopyTo(buffer, 0);

            Transceive(true, buffer);

            return ReadFromFifo() == Acknowledge;
        }


        private void Authenticate(byte command, byte blockNumber, Uid uid, byte[] key)
        {
            // Put reader in Idle mode
            WriteRegister(Command, Idle);

            // Clear the FIFO
            SetRegisterBits(FifoLevel, 0x80);

            // Create Authentication packet
            var data = new byte[12];
            data[0] = command;
            data[1] = (byte)(blockNumber & 0xFF);
            key.CopyTo(data, 2);
            uid.Bytes.CopyTo(data, 8);

            WriteToFifo(data);

            // Put reader in MfAuthent mode
            WriteRegister(Command, MifareAuthenticate);

            // Wait for (a generous) 25 ms
            Task.Delay(25).Wait();
        }

        private void Transceive(bool enableCrc, params byte[] data)
        {
            if (enableCrc)
            {
                // Enable CRC
                SetRegisterBits(TxMode, 0x80);
                SetRegisterBits(RxMode, 0x80);
            }

            // Put reader in Idle mode
            WriteRegister(Command, Idle);

            // Clear the FIFO
            SetRegisterBits(FifoLevel, 0x80);

            // Write the data to the FIFO
            WriteToFifo(data);

            // Put reader in Transceive mode and start sending
            WriteRegister(Command, MifareTransceive);
            SetRegisterBits(BitFraming, 0x80);

            // Wait for (a generous) 25 ms
            System.Threading.Tasks.Task.Delay(25).Wait();

            // Stop sending
            ClearRegisterBits(BitFraming, 0x80);
            
            if (!enableCrc) return;

            // Disable CRC
            ClearRegisterBits(TxMode, 0x80);
            ClearRegisterBits(RxMode, 0x80);
        }


        private byte[] ReadFromFifo(int length)
        {
            var buffer = new byte[length];

            for (int i = 0; i < length; i++)
                buffer[i] = ReadRegister(FifoData);

            return buffer;
        }

        private byte ReadFromFifo()
        {
            return ReadFromFifo(1)[0];
        }

        private void WriteToFifo(params byte[] values)
        {
            foreach (var b in values)
                WriteRegister(FifoData, b);
        }

        private int GetFifoLevel()
        {
            return ReadRegister(FifoLevel);
        }


        private byte ReadRegister(byte register)
        {
            register <<= 1;
            register |= 0x80;

            var writeBuffer = new byte[] { register, 0x00 };

            return TransferSpi(writeBuffer)[1];
        }

        private ushort ReadFromFifoShort()
        {
            var low = ReadRegister(FifoData);
            var high = (ushort)(ReadRegister(FifoData) << 8);

            return (ushort)(high | low);
        }

        private void WriteRegister(byte register, byte value)
        {
            register <<= 1;
            var writeBuffer = new byte[] { register, value };

            TransferSpi(writeBuffer);
        }

        private void SetRegisterBits(byte register, byte bits)
        {
            var currentValue = ReadRegister(register);
            WriteRegister(register, (byte)(currentValue | bits));
        }

        private void ClearRegisterBits(byte register, byte bits)
        {
            var currentValue = ReadRegister(register);
            WriteRegister(register, (byte)(currentValue & ~bits));
        }


        private byte[] TransferSpi(byte[] writeBuffer)
        {
            var readBuffer = new byte[writeBuffer.Length];
            Spi.TransferFullDuplex(writeBuffer, readBuffer);

            return readBuffer;
        }
    }
}
