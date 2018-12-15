import debug = require('debug');
import express = require('express');
import path = require('path');
import cors = require('cors');
import { EventHubClient, EventPosition } from '@azure/event-hubs';
//import * as dotenv from "dotenv";

import routes from "./routes/index";
import users from "./routes/user";

var app = express();
app.use(cors());
app.options("*", cors());

var storageName = "stingstorage";
var storageKey = "9YN+eDdjocIPd64VOPmUVMpo2c+FE+nOyxXPa9nzqxqKtzLs4AgGYX+jA6+zTEhs8xaih0na2Z2vmSgeWiXtgA==";
var connectionString = "HostName=StructureMonitoring.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=unYHBx8mNUkOu7jFAhBG4sTkL86e6J9gxaygI/QkeUI=";
var lastTelemetryData = "No Data received yet!";
var deviceEnes = lastTelemetryData;
var deviceRobin = lastTelemetryData;
var deviceMarc = lastTelemetryData;
var deviceBoris = lastTelemetryData;
import azure = require("azure-storage");
var tableService = azure.createTableService(storageName, storageKey);

// run function to read from azure storage table

var obj;

function readIotHub(connectionString) {
    var printError = err => {
        console.log(err.message);
    };

    var printMessage = message => {

        console.log("Last telemetry received: ");
        lastTelemetryData = JSON.stringify(message.body).substring(0, JSON.stringify(message.body).length - 1);
        lastTelemetryData = lastTelemetryData.concat(",");

        obj = JSON.parse(JSON.stringify(message.annotations));
        lastTelemetryData = lastTelemetryData.concat(`"DeviceId":"${obj["iothub-connection-device-id"]}"}`);
        console.log(lastTelemetryData);
        if (obj["iothub-connection-device-id"] === "RasPi_Enes")
            deviceEnes = lastTelemetryData;
        else if (obj["iothub-connection-device-id"] === "RasPi_Robin") {
            deviceRobin = lastTelemetryData;
        }
        else if (obj["iothub-connection-device-id"] === "RasPi_Marc")
            deviceMarc = lastTelemetryData;
        else if (obj["iothub-connection-device-id"] === "RasPi_Boris")
            deviceBoris = lastTelemetryData;
        else
            console.log("Device Id can not be recognized! Please check your DeviceId!");

    };

    var ehClient: EventHubClient;
    EventHubClient.createFromIotHubConnectionString(connectionString).then(client => {
        console.log("Successfully created the EventHub Client from iothub connection string.");
        ehClient = client;
        return ehClient.getPartitionIds();
    }).then(ids => {
        console.log("The partition ids are: ", ids);
        return ids.map(id => ehClient.receive(id, printMessage, printError, { eventPosition: EventPosition.fromEnqueuedTime(Date.now()) }));
    }).catch(printError);
}

readIotHub(connectionString);


//var device = 'RasPi_Enes';

// Gets the telemetry data in real time from the iotHub. This functions runs in the background
// since the start of the backend application.

app.get("/telemetry/current/RasPi_Enes", (req, res) => res.send(deviceEnes));

app.get("/telemetry/current/RasPi_Robin", (req, res) => res.send(deviceRobin));

app.get("/telemetry/current/RasPi_Marc", (req, res) => res.send(deviceMarc));

app.get("/telemetry/current/RasPi_Boris", (req, res) => res.send(deviceBoris));

// Gets the telemetry data from all registered devices in real time from the iotHub
app.get("/telemetry/current/all", (req, res) => {
    var response = new Array();
    var devices = [deviceEnes, deviceRobin, deviceMarc, deviceBoris];
    for (let device in devices) {
        if (devices.hasOwnProperty(device)) {
            if (devices[device] !== "No Data received yet!")
                response.push(devices[device]);
        }
    }
    res.send(JSON.stringify(eval(`[${response}]`)));
});


// Gets all the telemetry data from the last 24 hours from the azure-storage-table
// :device specifies the device Id of the gadget you want the selection from.

app.get("/telemetry/lastday/:device", (req, res) => {

  //var lastDay = Date.now() - 86400000;
  var query = new azure.TableQuery()
      .select(["unixtime", "temperature", "humidity", "altitude", "deviceid" ])
    //.top(10)
      .where("PartitionKey gt ?", (Date.now() - 86400000).toString())
      .and("deviceid eq ?", req.params.device);

  tableService.queryEntities("stingtablev3", query, null, (error, result, response) => {
      if (!error) {
          // result.entries contains entities matching the query

          console.log(result.entries);


          res.send(result.entries);
          //console.log(query);
      }

  });
});

// Gets the telemetry data of last week.

app.get("/telemetry/lastweek/:device", (req, res) => {

  //var lastDay = Date.now() - 86400000;
  var query = new azure.TableQuery()
      .select(["unixtime", "temperature", "humidity", "altitude", "deviceid"])
    //.top(10)
    .where("PartitionKey gt ?", (Date.now() - (86400000 * 7)).toString())
    .and("deviceid eq ?", req.params.device);

  tableService.queryEntities("stingtablev3", query, null, (error, result, response) => {
      if (!error) {
          // result.entries contains entities matching the query
          console.log(result.entries);
          res.send(result.entries);
          //console.log(query);
      }

  });
});

// Gets the telemetry data of last month.

app.get("/telemetry/lastmonth/:device", (req, res) => {

  var query = new azure.TableQuery()
      .select(["unixtime", "temperature", "humidity", "altitude", "deviceid"])
    //.top(10)
    .where("PartitionKey gt ?", (Date.now() - (86400000 * 30)).toString())
    .and("deviceid eq ?", req.params.device);

  tableService.queryEntities('stingtablev3', query, null, (error, result, response) => {
      if (!error) {
          // result.entries contains entities matching the query
          console.log(result.entries);
          res.send(result.entries);
          //console.log(query);
      }

  });
});

// view engine setup
app.set("views", path.join(__dirname, "views"));
app.set("view engine", "pug");

app.use(express.static(path.join(__dirname, "public")));

app.use("/", routes);
app.use("/users", users);

// catch 404 and forward to error handler
app.use((req, res, next) => {
    var err = new Error("Not Found");
    err["status"] = 404;
    next(err);
});

// error handlers

// development error handler
// will print stacktrace
if (app.get("env") === "development") {
    app.use((err: any, req, res, next) => {
        res.status(err["status"] || 500);
        res.render("error", {
            message: err.message,
            error: err
        });
    });
}

// production error handler
// no stacktraces leaked to user
app.use((err: any, req, res, next) => {
    res.status(err.status || 500);
    res.render("error", {
        message: err.message,
        error: {}
    });
});

app.set("port", process.env.PORT || 3000);

var server = app.listen(app.get("port"), () => {
    debug(`Express server listening on port ${server.address().port}`);
});