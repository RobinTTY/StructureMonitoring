# Structure Monitoring [![Build Status](https://dev.azure.com/muellerobin95/Systecs%20Structure%20Monitoring/_apis/build/status/RobinTTY.StructureMonitoring)](https://dev.azure.com/muellerobin95/Systecs%20Structure%20Monitoring/_build/latest?definitionId=3)

Monitor your home, office or whatever you can think of in terms of temperature, humidity and more. With the help of a Raspberry Pi and Azure this web based application gives you a quick overview of room telemetry data. Start with one Pi and go to as many as you like with minimal setup.

## API Documentation

API documentation is hosted on github pages and is generated by DocFX.

### Prerequisites

- At least one Raspberry Pi
- [Microsoft Azure Account](https://azure.microsoft.com/)

### Installing

- Install [Windows IoT Core](https://www.microsoft.com/en-us/software-download/windows10iotcore#!) on your Raspberry Pi

- Install [Node.js](https://nodejs.org/en/) and [Angular](https://angular.io/guide/quickstart)

- Build and deploy the Sting.Measurements App to your Raspberry Pi

- Set up an Azure IoT Hub and add your Pi to the devices

- Copy the generated connection string to the file path of your deployed app (you can change this by manipulating the AzureIotHub.cs file):

  - ```\Sting.Measurements-uwp_gk6cf97c3a7py\LocalState\DeviceConnectionString.txt```

- Forward the messages from your IoT Hub to a storage solution (e.g. Azure Table Storage)

- For instructions on setting up the client app see the README of the StingApp directory

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details