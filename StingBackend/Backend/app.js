"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const debug = require("debug");
const express = require("express");
const path = require("path");
const event_hubs_1 = require("@azure/event-hubs");
const index_1 = require("./routes/index");
const user_1 = require("./routes/user");
var app = express();
var storageName = 'stingstorage';
var storageKey = '9YN+eDdjocIPd64VOPmUVMpo2c+FE+nOyxXPa9nzqxqKtzLs4AgGYX+jA6+zTEhs8xaih0na2Z2vmSgeWiXtgA==';
var connectionString = 'HostName=StructureMonitoring.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=unYHBx8mNUkOu7jFAhBG4sTkL86e6J9gxaygI/QkeUI=';
var lastTelemetryData = 'No Data received yet!';
var DeviceEnes = lastTelemetryData;
var DeviceRobin = lastTelemetryData;
var DeviceMarc = lastTelemetryData;
var DeviceBoris = lastTelemetryData;
var azure = require('azure-storage');
var tableService = azure.createTableService(storageName, storageKey);
// run function to read from azure storage table
var obj;
function readIotHub(connectionString) {
    var printError = function (err) {
        console.log(err.message);
    };
    var printMessage = function (message) {
        console.log("Last telemetry received: ");
        lastTelemetryData = JSON.stringify(message.body).substring(0, JSON.stringify(message.body).length - 1);
        lastTelemetryData = lastTelemetryData.concat(',');
        obj = JSON.parse(JSON.stringify(message.annotations));
        lastTelemetryData = lastTelemetryData.concat('"DeviceId":"' + obj['iothub-connection-device-id'] + '"}');
        //console.log(lastTelemetryData);
        if (obj['iothub-connection-device-id'] = 'RasPi_Enes')
            DeviceEnes = lastTelemetryData;
        else if (obj['iothub-connection-device-id'] = 'RasPi_Robin')
            DeviceRobin = lastTelemetryData;
        else if (obj['iothub-connection-device-id'] = 'RasPi_Marc')
            DeviceMarc = lastTelemetryData;
        else if (obj['iothub-connection-device-id'] = 'RasPi_Boris')
            DeviceBoris = lastTelemetryData;
        else
            console.log('Device Id can not be recognized! Please check your DeviceId!');
    };
    var ehClient;
    event_hubs_1.EventHubClient.createFromIotHubConnectionString(connectionString).then(function (client) {
        console.log("Successully created the EventHub Client from iothub connection string.");
        ehClient = client;
        return ehClient.getPartitionIds();
    }).then(function (ids) {
        console.log("The partition ids are: ", ids);
        return ids.map(function (id) {
            return ehClient.receive(id, printMessage, printError, { eventPosition: event_hubs_1.EventPosition.fromEnqueuedTime(Date.now()) });
        });
    }).catch(printError);
}
readIotHub(connectionString);
//var device = 'RasPi_Enes';
// Gets the telemetry data in real time from the iotHub. This functions runs in the background
// since the start of the backend application.
app.get('/telemetry/current/RasPi_Enes', (req, res) => res.send(DeviceEnes));
app.get('/telemetry/current/RasPi_Robin', (req, res) => res.send(DeviceRobin));
app.get('/telemetry/current/RasPi_Marc', (req, res) => res.send(DeviceMarc));
app.get('/telemetry/current/RasPi_Boris', (req, res) => res.send(DeviceBoris));
// Gets all the telemetry data from the last 24 hours from the azure-storage-table
// :device specifies the device Id of the gadget you want the selection from.
app.get('/telemetry/lastday/:device', (req, res) => {
    //var lastDay = Date.now() - 86400000;
    var query = new azure.TableQuery()
        .select(['Timestamp', 'temperature', 'humidity', 'altitude', 'deviceid'])
        //.top(10)
        .where('PartitionKey gt ?', (Date.now() - 86400000).toString())
        .and('deviceid eq ?', req.params.device);
    tableService.queryEntities('stingtablev3', query, null, function (error, result, response) {
        if (!error) {
            // result.entries contains entities matching the query
            console.log(result.entries);
            res.send(result.entries);
            //console.log(query);
        }
    });
});
// Gets the telemetry data of last week.
app.get('/telemetry/lastweek/:device', (req, res) => {
    //var lastDay = Date.now() - 86400000;
    var query = new azure.TableQuery()
        .select(['Timestamp', 'temperature', 'humidity', 'altitude', 'deviceid'])
        //.top(10)
        .where('PartitionKey gt ?', (Date.now() - (86400000 * 7)).toString())
        .and('deviceid eq ?', req.params.device);
    tableService.queryEntities('stingtablev3', query, null, function (error, result, response) {
        if (!error) {
            // result.entries contains entities matching the query
            console.log(result.entries);
            res.send(result.entries);
            //console.log(query);
        }
    });
});
// Gets the telemetry data of last month.
app.get('/telemetry/lastmonth/:device', (req, res) => {
    var query = new azure.TableQuery()
        .select(['Timestamp', 'temperature', 'humidity', 'altitude', 'deviceid'])
        .top(10)
        .where('PartitionKey gt ?', (Date.now() - (86400000 * 30)).toString())
        .and('deviceid eq ?', req.params.device);
    tableService.queryEntities('stingtablev3', query, null, function (error, result, response) {
        if (!error) {
            // result.entries contains entities matching the query
            console.log(result.entries);
            res.send(result.entries);
            //console.log(query);
        }
    });
});
// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'pug');
app.use(express.static(path.join(__dirname, 'public')));
app.use('/', index_1.default);
app.use('/users', user_1.default);
// catch 404 and forward to error handler
app.use(function (req, res, next) {
    var err = new Error('Not Found');
    err['status'] = 404;
    next(err);
});
// error handlers
// development error handler
// will print stacktrace
if (app.get('env') === 'development') {
    app.use((err, req, res, next) => {
        res.status(err['status'] || 500);
        res.render('error', {
            message: err.message,
            error: err
        });
    });
}
// production error handler
// no stacktraces leaked to user
app.use((err, req, res, next) => {
    res.status(err.status || 500);
    res.render('error', {
        message: err.message,
        error: {}
    });
});
app.set('port', process.env.PORT || 3000);
var server = app.listen(app.get('port'), function () {
    debug('Express server listening on port ' + server.address().port);
});
//# sourceMappingURL=app.js.map