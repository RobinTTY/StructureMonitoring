# Backend Installation
 
 Please navigate to the root folder and use the command 'npm install' to install all the necessary packages.
 Then, you are able to build the project with Visual Studio.
 
 # Description
  
  This program has two main purposes:
  
  - The Event-Hub Client is a background application, which gets data from the Iot-Hub every second
  
  - The Azure-Storage table functions return telemetry data of last day, last week and last month from the 
    Azure database

  Both modules can be obtained by the front-end with a get request.

# Usage

The following get requests are available:

- [Device] = RasPi_Marc, RasPi_Enes, RasPi_Robin, RasPi_Boris

- /telemetry/current/[Device]:  Returns the current telemetry data from the IoT-Hub 
  
- /telemetry/lastday/[Device]:  Returns all telemetry data recorded in the last 24 hours

- /telemetry/lastweek/[Device]: Returns all telemetry data recorded in the last 7 days

- /telemetry/lastmonth/[Device]: Return all telemetry data recorded in the last 30 days

# Attention

The stream analytics resource has to be running, otherwise the data is not transfered to the database!
  
  
