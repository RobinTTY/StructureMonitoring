# Sting - Structure Monitoring
This project aims to provide the required services to set up the monitoring of environmental data for one or several buildings/objects. The structure of the project divides the functionality into 3 distinct application, which are:

- *Sting.Measurements:* .Net Core 3.0 application, deployed to a device able to operate sensors like the Raspberry Pi. Used for performing the environmental measurements.
- *Sting.Backend:* ASP.NET Core 3.0 application used to host a web API to access sensor data from multiple devices through a MongoDB database.
- *Sting.WebApp:* Angular 7 application used to visualize the aquired environmental data. Responsively designed to be accessible on any device.

## Work in Progress

This project is being actively worked on and might not be in a runnable state.

## Prerequisites

- A device that is able to operate sensors and is supported by the .NetCore runtime (the application is tested on a Raspberry Pi 3B+)
- MongoDB Atlas Cluster (free option is sufficient) or local installation

## Installing

- It is recommended to use [Raspbian](https://www.raspberrypi.org/downloads/) or [Windows IoT Core](https://www.microsoft.com/en-us/software-download/windows10iotcore#!) on your measurement device
- Executables will be provided as soon as the project reaches the 1.0 milestone
- More instructions are coming soon...

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
