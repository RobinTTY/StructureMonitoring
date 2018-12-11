using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Sting.Cloud;
using Sting.Measurements.Components;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Measurements
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private readonly Bmp180 _pressureSensor = new Bmp180();
        private readonly Dht11 _tempSensor = new Dht11();
        private readonly Buzzer _buzzer = new Buzzer();
        private readonly Led _statusLed = new Led();
        private readonly Lcd _lcd = new Lcd();
        private readonly AzureIotHub _structureMonitoringHub = new AzureIotHub();
        private bool _cancelRequested;
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            InitComponentsAsync();
            InitDirectMethodCallsAsync();
            ThreadPoolTimer.CreatePeriodicTimer(PeriodicTask, TimeSpan.FromSeconds(10));
        }

        // initialize used components async
        // TODO: CHECK RETURN VALUE FOR SUCCESS
        private async void InitComponentsAsync()
        {
            await _tempSensor.InitComponentAsync(4);
            await _pressureSensor.InitComponentAsync();
            await _buzzer.InitComponentAsync(18);
            await _statusLed.InitComponentAsync(5);
            await _lcd.InitComponentAsync();
            _lcd.Write("Welcome to Sting");
            _lcd.SetCursorPosition(2);
            _lcd.Write("Initializing...");
        }

        // Register Direct Methods and set event handlers
        private async void InitDirectMethodCallsAsync()
        {
            await _structureMonitoringHub.RegisterDirectMethodsAsync();
            _structureMonitoringHub.Locate += _buzzer.OnLocate;
            _structureMonitoringHub.Terminate += OnTerminate;
        }

        // Task which is executed every x seconds as defined in Run()
        // Take Measurements periodically
        // TODO: Introduce Cloud to Device Message which can cancel the deferral, resulting in termination of the program
        private async void PeriodicTask(ThreadPoolTimer timer)
        {
            if (_cancelRequested == false)
            {
                var combinedData = new TelemetryData();
                var dhtTelemetry = await _tempSensor.TakeMeasurementAsync();
                var bmpTelemetry = await _pressureSensor.TakeMeasurementAsync();

                _lcd.ClearScreen();
                // Use bmp measurements (duplicates) over dht measurements because of higher accuracy
                if (dhtTelemetry == null && bmpTelemetry == null)
                    _lcd.Write("Invalid Measurement");
                else
                {
                    combinedData.Overwrite(dhtTelemetry);
                    combinedData.Overwrite(bmpTelemetry);
                    _lcd.Write($"Temp:{combinedData.Temperature:F1}{(char)223}C");    //prints the temp with one decimal place and the degree symbol
                    _lcd.SetCursorPosition(2);
                    _lcd.Write($"Hum:{combinedData.Humidity}% Alt:{combinedData.Altitude:F0}m");
                }
                
                var success = await _structureMonitoringHub.SendDeviceToCloudMessageAsync(combinedData.ToJson());
                Debug.WriteLine(success ? "Message sent!" : "Could not send your message.");
                _statusLed.Blink(5000);
            }
            else
            {
                _lcd.ClearScreen();
                _lcd.Write("Shutting down."); _lcd.SetCursorPosition(2); _lcd.Write("Goodbye ");
                // indicate that deferral is completed
                timer.Cancel();
                _deferral.Complete();
            }
        }

        private void OnTerminate(object source, EventArgs e)
        {
            Debug.WriteLine("Termination was requested. Shutting Down.");
            _cancelRequested = true;
        }
    }
}