import debug = require('debug');
import express = require('express');
import path = require('path');
import { EventHubClient, EventPosition } from '@azure/event-hubs';
import * as dotenv from "dotenv";

import routes from './routes/index';
import users from './routes/user';

var app = express();

var storageName = 'stingstorage';
var storageKey = '9YN+eDdjocIPd64VOPmUVMpo2c+FE+nOyxXPa9nzqxqKtzLs4AgGYX+jA6+zTEhs8xaih0na2Z2vmSgeWiXtgA==';
var connectionString = 'HostName=StructureMonitoring.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=unYHBx8mNUkOu7jFAhBG4sTkL86e6J9gxaygI/QkeUI=';
var lastTelemetryData = 'No Data received yet!'
var query = 'No result yet!'
var azure = require('azure-storage');
var tableService = azure.createTableService(storageName, storageKey);

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
    
    var tableService = azure.createTableService(storageName, storageKey);
    tableService.retrieveEntity('stingtable', 'RasPi_Enes', '2018-11-15T19:34:22.0411843+00:00', function (error, result, response) {
        if (!error) {

        }
        // console.log(response);
        console.log(result);
    });
    */
      var azure = require('azure-storage');
  var tableService = azure.createTableService(storageName, storageKey);
  var query = new azure.TableQuery()
    .top(10)
    .where('RowKey gt ?', '22');

      tableService.queryEntities('stingtablev2', query, null, function (error, result, response) {
        if (!error) {
          // result.entries contains entities matching the query
          console.log(result.entries);
          query = result.entries;
          return query;
          //console.log(query);
        }
        
  });
  

}
// run function to read from azure storage table
 //azureImport(storageName, storageKey);

var obj;

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
        lastTelemetryData = lastTelemetryData.concat(',');

      obj = JSON.parse(JSON.stringify(message.annotations));
      lastTelemetryData = lastTelemetryData.concat('"DeviceId":"'+ obj['iothub-connection-device-id'] + '"}');
        console.log(lastTelemetryData);

    };

    var ehClient;
    EventHubClient.createFromIotHubConnectionString(connectionString).then(function (client) {
        console.log("Successully created the EventHub Client from iothub connection string.");
        ehClient = client;
        return ehClient.getPartitionIds();
    }).then(function (ids) {
        console.log("The partition ids are: ", ids);
        return ids.map(function (id) {
            return ehClient.receive(id, printMessage, printError, { eventPosition: EventPosition.fromEnqueuedTime(Date.now()) });
        });
    }).catch(printError);
}

readIotHub(connectionString);

console.log(Date.now());

var device = 'RasPi_Enes';

// Gets the telemetry data in real time from the iotHub. This functions runs in the background
// since the start of the backend application.

app.get('/telemetry/current', (req, res) => res.send(lastTelemetryData));

// Gets all the telemetry data from the last 24 hours from the azure-storage-table
// :device specifies the device Id of the gadget you want the selection from.

app.get('/telemetry/lastday/:device', (req, res) => {

  //var lastDay = Date.now() - 86400000;
  var query = new azure.TableQuery()
    .select(['Timestamp', 'temperature', 'humidity', 'altitude', 'deviceid'])
    //.top(10)
    .where('RowKey gt ?', (Date.now() - (86400000 * 30)).toString())
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
    .where('RowKey gt ?', (Date.now() - (86400000 * 7)).toString())
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
    .where('RowKey gt ?', (Date.now() - (86400000 * 30)).toString())
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

app.use('/', routes);
app.use('/users', users);

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
    app.use((err: any, req, res, next) => {
        res.status(err['status'] || 500);
        res.render('error', {
            message: err.message,
            error: err
        });
    });
}

// production error handler
// no stacktraces leaked to user
app.use((err: any, req, res, next) => {
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

