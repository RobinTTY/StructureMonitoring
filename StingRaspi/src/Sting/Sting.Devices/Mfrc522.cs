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
        private static class Registers
        {
            private const byte bitFraming = 0x0D;
            private const byte comIrq = 0x04;
            private const byte comIrqEnable = 0x02;
            private const byte command = 0x01;
            private const byte control = 0x0C;
            private const byte error = 0x06;
            private const byte fifoData = 0x09;
            private const byte fifoLevel = 0x0A;
            private const byte mode = 0x11;
            private const byte rxMode = 0x13;
            private const byte timerMode = 0x2A;
            private const byte timerPrescaler = 0x2B;
            private const byte timerReloadHigh = 0x2C;
            private const byte timerReloadLow = 0x2D;
            private const byte txAsk = 0x15;
            private const byte txControl = 0x14;
            private const byte txMode = 0x12;
            private const byte version = 0x37;

            public static byte BitFraming => bitFraming;
            public static byte ComIrq => comIrq;
            public static byte ComIrqEnable => comIrqEnable;
            public static byte Command => command;
            public static byte Control => control;
            public static byte Error => error;
            public static byte FifoData => fifoData;
            public static byte FifoLevel => fifoLevel;
            public static byte Mode => mode;
            public static byte RxMode => rxMode;
            public static byte TimerMode => timerMode;
            public static byte TimerPrescaler => timerPrescaler;
            public static byte TimerReloadHigh => timerReloadHigh;
            public static byte TimerReloadLow => timerReloadLow;
            public static byte TxAsk => txAsk;
            public static byte TxControl => txControl;
            public static byte TxMode => txMode;
            public static byte Version => version;
        }

        private static class PiccResponses
        {
            private const ushort answerToRequest = 0x0004;
            private const byte selectAcknowledge = 0x08;
            private const byte acknowledge = 0x0A;

            public static byte Acknowledge => acknowledge;
            public static byte SelectAcknowledge => selectAcknowledge;
            public static ushort AnswerToRequest => answerToRequest;
        }

        private static class PiccCommands
        {
            private const byte anticollision_1 = 0x93;
            private const byte anticollision_2 = 0x20;
            private const byte authenticateKeyA = 0x60;
            private const byte authenticateKeyB = 0x61;
            private const byte halt_1 = 0x50;
            private const byte halt_2 = 0x00;
            private const byte read = 0x30;
            private const byte request = 0x26;
            private const byte select_1 = 0x93;
            private const byte select_2 = 0x70;
            private const byte write = 0xA0;

            public static byte AuthenticateKeyA => authenticateKeyA;
            public static byte AuthenticateKeyB => authenticateKeyB;
            public static byte Halt_1 => halt_1;
            public static byte Halt_2 => halt_2;
            public static byte Read => read;
            public static byte Request => request;
            public static byte Select_1 => select_1;
            public static byte Select_2 => select_2;
            public static byte Write => write;
            public static byte Anticollision_1 => anticollision_1;
            public static byte Anticollision_2 => anticollision_2;
        }

        private static class PcdCommands
        {
            private const byte idle = 0x00;
            private const byte mifareAuthenticate = 0x0E;
            private const byte transceive = 0x0C;

            public static byte Idle => idle;
            public static byte MifareAuthenticate => mifareAuthenticate;
            public static byte Transceive => transceive;
        }


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

        public class Mfrc522Test
        {
            private SpiDevice Spi { get; set; }
            private GpioController IoController { get; set; }
            private GpioPin ResetPowerDown { get; set; }

            /* Uncomment for Raspberry Pi 2 */
            private const string SPI_CONTROLLER_NAME = "SPI0";
            private const Int32 SPI_CHIP_SELECT_LINE = 0;
            private const Int32 RESET_PIN = 25;

            public async Task InitIO()
            {

                try
                {
                    IoController = GpioController.GetDefault();

                    ResetPowerDown = IoController.OpenPin(RESET_PIN);
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
                    var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE)
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
                WriteRegister(Registers.TxAsk, 0x40);

                // Set CRC to 0x6363
                WriteRegister(Registers.Mode, 0x3D);

                // Enable antenna
                SetRegisterBits(Registers.TxControl, 0x03);
            }


            public bool IsTagPresent()
            {
                // Enable short frames
                WriteRegister(Registers.BitFraming, 0x07);

                // Transceive the Request command to the tag
                Transceive(false, PiccCommands.Request);

                // Disable short frames
                WriteRegister(Registers.BitFraming, 0x00);

                // Check if we found a card
                return GetFifoLevel() == 2 && ReadFromFifoShort() == PiccResponses.AnswerToRequest;
            }

            public Uid ReadUid()
            {
                // Run the anti-collision loop on the card
                Transceive(false, PiccCommands.Anticollision_1, PiccCommands.Anticollision_2);

                // Return tag UID from FIFO
                return new Uid(ReadFromFifo(5));
            }

            public void HaltTag()
            {
                // Transceive the Halt command to the tag
                Transceive(false, PiccCommands.Halt_1, PiccCommands.Halt_2);
            }

            public bool SelectTag(Uid uid)
            {
                // Send Select command to tag
                var data = new byte[7];
                data[0] = PiccCommands.Select_1;
                data[1] = PiccCommands.Select_2;
                uid.FullUid.CopyTo(data, 2);

                Transceive(true, data);

                return GetFifoLevel() == 1 && ReadFromFifo() == PiccResponses.SelectAcknowledge;
            }

            internal byte[] ReadBlock(byte blockNumber, Uid uid, byte[] keyA = null, byte[] keyB = null)
            {
                if (keyA != null)
                    MifareAuthenticate(PiccCommands.AuthenticateKeyA, blockNumber, uid, keyA);
                else if (keyB != null)
                    MifareAuthenticate(PiccCommands.AuthenticateKeyB, blockNumber, uid, keyB);
                else
                    return null;

                // Read block
                Transceive(true, PiccCommands.Read, blockNumber);

                return ReadFromFifo(16);
            }

            internal bool WriteBlock(byte blockNumber, Uid uid, byte[] data, byte[] keyA = null, byte[] keyB = null)
            {
                if (keyA != null)
                    MifareAuthenticate(PiccCommands.AuthenticateKeyA, blockNumber, uid, keyA);
                else if (keyB != null)
                    MifareAuthenticate(PiccCommands.AuthenticateKeyB, blockNumber, uid, keyB);
                else
                    return false;

                // Write block
                Transceive(true, PiccCommands.Write, blockNumber);

                if (ReadFromFifo() != PiccResponses.Acknowledge)
                    return false;

                // Make sure we write only 16 bytes
                var buffer = new byte[16];
                data.CopyTo(buffer, 0);

                Transceive(true, buffer);

                return ReadFromFifo() == PiccResponses.Acknowledge;
            }


            private void MifareAuthenticate(byte command, byte blockNumber, Uid uid, byte[] key)
            {
                // Put reader in Idle mode
                WriteRegister(Registers.Command, PcdCommands.Idle);

                // Clear the FIFO
                SetRegisterBits(Registers.FifoLevel, 0x80);

                // Create Authentication packet
                var data = new byte[12];
                data[0] = command;
                data[1] = (byte)(blockNumber & 0xFF);
                key.CopyTo(data, 2);
                uid.Bytes.CopyTo(data, 8);

                WriteToFifo(data);

                // Put reader in MfAuthent mode
                WriteRegister(Registers.Command, PcdCommands.MifareAuthenticate);

                // Wait for (a generous) 25 ms
                System.Threading.Tasks.Task.Delay(25).Wait();
            }

            private void Transceive(bool enableCrc, params byte[] data)
            {
                if (enableCrc)
                {
                    // Enable CRC
                    SetRegisterBits(Registers.TxMode, 0x80);
                    SetRegisterBits(Registers.RxMode, 0x80);
                }

                // Put reader in Idle mode
                WriteRegister(Registers.Command, PcdCommands.Idle);

                // Clear the FIFO
                SetRegisterBits(Registers.FifoLevel, 0x80);

                // Write the data to the FIFO
                WriteToFifo(data);

                // Put reader in Transceive mode and start sending
                WriteRegister(Registers.Command, PcdCommands.Transceive);
                SetRegisterBits(Registers.BitFraming, 0x80);

                // Wait for (a generous) 25 ms
                System.Threading.Tasks.Task.Delay(25).Wait();

                // Stop sending
                ClearRegisterBits(Registers.BitFraming, 0x80);
                
                if (!enableCrc) return;

                // Disable CRC
                ClearRegisterBits(Registers.TxMode, 0x80);
                ClearRegisterBits(Registers.RxMode, 0x80);
            }


            private byte[] ReadFromFifo(int length)
            {
                var buffer = new byte[length];

                for (int i = 0; i < length; i++)
                    buffer[i] = ReadRegister(Registers.FifoData);

                return buffer;
            }

            private byte ReadFromFifo()
            {
                return ReadFromFifo(1)[0];
            }

            private void WriteToFifo(params byte[] values)
            {
                foreach (var b in values)
                    WriteRegister(Registers.FifoData, b);
            }

            private int GetFifoLevel()
            {
                return ReadRegister(Registers.FifoLevel);
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
                var low = ReadRegister(Registers.FifoData);
                var high = (ushort)(ReadRegister(Registers.FifoData) << 8);

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
}
