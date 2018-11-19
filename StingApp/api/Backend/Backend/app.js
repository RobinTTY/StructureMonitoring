"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const debug = require("debug");
const express = require("express");
const path = require("path");
const azure = require("azure-storage");
const event_hubs_1 = require("@azure/event-hubs");
const index_1 = require("./routes/index");
const user_1 = require("./routes/user");
var app = express();
var storageName = 'stingstorage';
var storageKey = '9YN+eDdjocIPd64VOPmUVMpo2c+FE+nOyxXPa9nzqxqKtzLs4AgGYX+jA6+zTEhs8xaih0na2Z2vmSgeWiXtgA==';
var connectionString = 'HostName=StructureMonitoring.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=unYHBx8mNUkOu7jFAhBG4sTkL86e6J9gxaygI/QkeUI=';
var lastTelemetryData = 'No Data received yet!';
function azureImport(storageName, storageKey) {
    //var azure = require('azure-storage');
    /*
    var tableSvc = azure.createTableService(storageName, storageKey);
    tableSvc.createTableIfNotExists('mytable2', function (error, result, response) {
        if (!error) {
        }
        console.log(response);
        console.log(result);
    });
    
    tableSvc.retrieveEntity('stingtable', 'PartitionKey', 'RowKey', function (error, result, response) {
        if (!error) {
            
        }
        console.log(response);
        console.log(result);
    });
    


    var tableService = azure.createTableService(storageName, storageKey);
    var entGen = azure.TableUtilities.entityGenerator;
    var task = {
        PartitionKey: entGen.String('hometasks'),
        RowKey: entGen.String('1'),
        description: entGen.String('take out the trash'),
        dueDate: entGen.DateTime(new Date(Date.UTC(2015, 6, 20))),
    };
    tableService.insertEntity('mytable', task, function (error, result, response) {
        if (!error) {
            // result contains the ETag for the new entity
        }
    });
    */
    var tableService = azure.createTableService(storageName, storageKey);
    tableService.retrieveEntity('stingtable', 'RasPi_Enes', '2018-11-15T19:34:22.0411843+00:00', function (error, result, response) {
        if (!error) {
        }
        // console.log(response);
        console.log(result);
    });
}
// run function to read from azure storage table
// azureImport(storageName, storageKey);
function readIotHub(connectionString) {
    var printError = function (err) {
        console.log(err.message);
    };
    var printMessage = function (message) {
        /*
        console.log('Telemetry received: ');
        console.log(JSON.stringify(message.body));
        console.log('Application properties (set by device): ')
        console.log(JSON.stringify(message.applicationProperties));
        console.log('System properties (set by IoT Hub): ')
        console.log(JSON.stringify(message.annotations));
        console.log('');
        */
        console.log("Last telemetry received: ");
        lastTelemetryData = JSON.stringify(message.body).substring(0, JSON.stringify(message.body).length - 1);
        lastTelemetryData = lastTelemetryData.concat(', ' + JSON.stringify(message.annotations).substring(1));
        console.log(lastTelemetryData);
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
app.get('/', (req, res) => res.send(lastTelemetryData));
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