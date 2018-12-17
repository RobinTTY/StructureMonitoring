"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const debug = require("debug");
const express = require("express");
const path = require("path");
const cors = require("cors");
const event_hubs_1 = require("@azure/event-hubs");
const index_1 = require("./routes/index");
const user_1 = require("./routes/user");
const azure = require("azure-storage");
// Setting up some environment variables
var storageName = "stingstorage"; // Name of the storage in Azure
var table = 'mytable'; // Name of the table in the storage
var storageKey = "9YN+eDdjocIPd64VOPmUVMpo2c+FE+nOyxXPa9nzqxqKtzLs4AgGYX+jA6+zTEhs8xaih0na2Z2vmSgeWiXtgA=="; // Key of the azure storage
var skippedData = 5; //Every fifth measurement is saved to the database
var connectionString = "HostName=StructureMonitoring.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=unYHBx8mNUkOu7jFAhBG4sTkL86e6J9gxaygI/QkeUI="; // Primary connection string of the azure storage
var lastTelemetryData = "No Data received yet!"; // Telemetry variable 
var deviceEnes = lastTelemetryData;
var deviceRobin = lastTelemetryData;
var deviceMarc = lastTelemetryData;
var deviceBoris = lastTelemetryData;
var obj;
var T_obj;
var telemetryForAzure = {};
var dataBaseCycle = [];
dataBaseCycle[0] = skippedData;
dataBaseCycle[1] = skippedData;
dataBaseCycle[2] = skippedData;
dataBaseCycle[3] = skippedData;
// Create the table service
var tableService = azure.createTableService(storageName, storageKey);
var app = express();
app.use(cors());
app.options("*", cors());
// The following function starts a client, which looks for incoming data from the iot-hub.
// Furthermore, it saves the data to the corresponding device variable.
// It also inserts the data to the azure storage table. The cycle is specified in the skippedData variable
function readIotHub(connectionString) {
    var printError = err => {
        console.log(err.message);
    };
    var printMessage = message => {
        console.log("Last telemetry received: ");
        // Incoming telemetry json is modified to our needs and then saved to the corresponding deviceId
        lastTelemetryData = JSON.stringify(message.body).substring(0, JSON.stringify(message.body).length - 1);
        lastTelemetryData = lastTelemetryData.concat(",");
        obj = JSON.parse(JSON.stringify(message.annotations));
        lastTelemetryData = lastTelemetryData.concat(`"DeviceId":"${obj["iothub-connection-device-id"]}"}`);
        console.log(lastTelemetryData);
        T_obj = JSON.parse(lastTelemetryData);
        console.log(dataBaseCycle);
        if (obj["iothub-connection-device-id"] === "RasPi_Enes") {
            deviceEnes = lastTelemetryData;
            dataBaseCycle[0]--;
        }
        else if (obj["iothub-connection-device-id"] === "RasPi_Robin") {
            deviceRobin = lastTelemetryData;
            dataBaseCycle[1]--;
        }
        else if (obj["iothub-connection-device-id"] === "RasPi_Marc") {
            deviceMarc = lastTelemetryData;
            dataBaseCycle[2]--;
        }
        else if (obj["iothub-connection-device-id"] === "Raspi_Boris") {
            deviceBoris = lastTelemetryData;
            dataBaseCycle[3]--;
        }
        else
            console.log("Device Id can not be recognized! Please check your DeviceId!");
        // Function to insert the current telemetry to the table
        telemetryForAzure = {
            PartitionKey: { '$': 'Edm.String', _: (T_obj.UnixTimeStampMilliseconds).toString() },
            RowKey: { '$': 'Edm.String', _: (T_obj.DeviceId).toString() },
            altitude: { _: (T_obj.Altitude).toString() },
            deviceid: { _: (T_obj.DeviceId).toString() },
            humidity: { '$': 'Edm.Double', _: (T_obj.Humidity).toString() },
            pressure: { _: (T_obj.Pressure).toString() },
            temperature: { _: (T_obj.Temperature).toString() },
            unixtime: { '$': 'Edm.Int64', _: (T_obj.UnixTimeStampMilliseconds).toString() }
        };
        if (dataBaseCycle[0] <= 0 || dataBaseCycle[1] <= 0 || dataBaseCycle[2] <= 0 || dataBaseCycle[3] <= 0) {
            tableService.insertEntity(table, telemetryForAzure, function (error, result, response) {
                if (!error) {
                    // console.log('Success! Check the database.');
                }
            });
            if (dataBaseCycle[0] <= 0)
                dataBaseCycle[0] = skippedData;
            else if (dataBaseCycle[1] <= 0)
                dataBaseCycle[1] = skippedData;
            else if (dataBaseCycle[2] <= 0)
                dataBaseCycle[2] = skippedData;
            else if (dataBaseCycle[3] <= 0)
                dataBaseCycle[3] = skippedData;
        }
    };
    var ehClient;
    event_hubs_1.EventHubClient.createFromIotHubConnectionString(connectionString).then(client => {
        console.log("Successfully created the EventHub Client from iothub connection string.");
        ehClient = client;
        return ehClient.getPartitionIds();
    }).then(ids => {
        console.log("The partition ids are: ", ids);
        return ids.map(id => ehClient.receive(id, printMessage, printError, { eventPosition: event_hubs_1.EventPosition.fromEnqueuedTime(Date.now()) }));
    }).catch(printError);
}
readIotHub(connectionString); // Starts the client, which will run in the background "infinitely"
// Frontend can access the data from the following get request uris
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
        .select(["unixtime", "temperature", "humidity", "altitude", "deviceid"])
        .where("PartitionKey gt ?", (Date.now() - 86400000).toString())
        .and("RowKey eq ?", req.params.device);
    tableService.queryEntities(table, query, null, (error, result, response) => {
        if (!error) {
            // result.entries contains entities matching the query
            res.send(result.entries);
        }
    });
});
// Gets the telemetry data of last week.
app.get("/telemetry/lastweek/:device", (req, res) => {
    //var lastDay = Date.now() - 86400000;
    var query = new azure.TableQuery()
        .select(["unixtime", "temperature", "humidity", "altitude", "deviceid"])
        .where("PartitionKey gt ?", (Date.now() - (86400000 * 7)).toString())
        .and("deviceid eq ?", req.params.device);
    tableService.queryEntities(table, query, null, (error, result, response) => {
        if (!error) {
            // result.entries contains entities matching the query
            res.send(result.entries);
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
    tableService.queryEntities(table, query, null, (error, result, response) => {
        if (!error) {
            // result.entries contains entities matching the query
            res.send(result.entries);
        }
    });
});
// handle Cloud to device messages
var Client = require("azure-iothub").Client;
var client = Client.fromConnectionString(connectionString);
app.get("/invoke/:deviceMethod/:device", (req, res) => {
    var methodParams = {
        methodName: req.params.deviceMethod,
        device: req.params.device,
        payload: null,
        responseTimeoutInSeconds: 30
    };
    client.invokeDeviceMethod(methodParams.device, methodParams, (err, result) => {
        if (err) {
            console.error(`Failed to invoke method '${methodParams.methodName}': ${err.message}`);
            res.status(405).end();
        }
        else {
            res.status(200).end();
        }
    });
});
// view engine setup
app.set("views", path.join(__dirname, "views"));
app.set("view engine", "pug");
app.use(express.static(path.join(__dirname, "public")));
app.use("/", index_1.default);
app.use("/users", user_1.default);
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
    app.use((err, req, res, next) => {
        res.status(err["status"] || 500);
        res.render("error", {
            message: err.message,
            error: err
        });
    });
}
// production error handler
// no stacktraces leaked to user
app.use((err, req, res, next) => {
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
//# sourceMappingURL=app.js.map