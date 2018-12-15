import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Observable } from 'rxjs';
import {ChartService} from '../chart.service';
import {Chart} from 'chart.js'
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router'
import { Alert } from 'selenium-webdriver';

//Get building data from local file
import * as json1 from '../buildings';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})

export class RoomComponent implements OnInit {

  urlSplit$: Array<string>;
  public jsonObject: any;
  bData$: Object;
  room$: Object;
  fetchResponse: boolean;
  
  chart = [];

  constructor(private _chart: ChartService, private service: TelemetryDataImportService, private route: ActivatedRoute, private http: HttpClient, private router: Router) { 
    this.route.params.subscribe(params => this.room$ = params.id)
  }

  // get configuration data for current room, fetch telemetry data
  ngOnInit() {    
    this.urlSplit$ = this.router.url.split('/')
    this.bData$ = json1.default.buildings[parseInt(this.urlSplit$[2]) - 1].floors[parseInt(this.urlSplit$[4]) - 1].rooms[parseInt(this.urlSplit$[6]) - 1];
    this.fetchTelemetry();
    
    this._chart.deviceData(this.bData$["device"])
      .subscribe(res => {
        
        let temperature = Object.values(res).map(res => res.temperature._);
        let humidity = Object.values(res).map(res => res.humidity._);
        let altitude = Object.values(res).map(res => res.altitude._);
        let dateTime = Object.values(res).map(res => res.unixtime._);

        let weatherDates = []
        dateTime.forEach((res) => {
          let jsdate = new Date (res * 1)
          weatherDates.push(jsdate.toLocaleTimeString('de-DE', {month: 'short',day: 'numeric', hour: 'numeric', minute: 'numeric'}))
          // To show year, month or day, use the following parameters
          // weatherDates.push(jsdate.toLocaleTimeString('de-DE', {year: 'numeric', month: 'short', day: 'numeric'}))
        })

        this.chart = new Chart('canvasTemp', {
          type: 'line',
          data: {
            labels: weatherDates,
            datasets: [
              {
                data: temperature,
                borderColor: '#e21212',
                fill: false
              },
            ]
          },
          options: {
            elements: {
              point: {
                radius: 0
              }
            },
            legend: {
              display: false
            },
            scales: {
              xAxes: [{
                display: true,
                scaleLabel: {
                  display: true,
                  labelString: 'Date'
                }
              }],
              yAxes: [{
                display: true,
                scaleLabel: {
                  display: true,
                  labelString: 'Temperature in °C'
                }
              }]
            }
          }
        })

        this.chart = new Chart('canvasHum', {
          type: 'line',
          data: {
            labels: weatherDates,
            datasets: [
              {
                data: humidity,
                borderColor: '#3cba9f',
                fill: false
              },
            ]
          },
          options: {
            elements: {
              point: {
                radius: 0
              }
            },
            legend: {
              display: false
            },
            scales: {
              xAxes: [{
                display: true,
                scaleLabel: {
                  display: true,
                  labelString: 'Date'
                }
              }],
              yAxes: [{
                display: true,
                scaleLabel: {
                  display: true,
                  labelString: 'Humidity in %'
                }
              }]
            }
          }
        })
    })
  }

  // insert measured values if available into cards
  ngDoCheck() {
    try{
      document.getElementById("TempVal").innerText = this.jsonObject["Temperature"].valueOf().toString().substr(0,5) + "°C"
      document.getElementById("HumVal").innerText = this.jsonObject["Humidity"].valueOf().toString() + "%"
      document.getElementById("PressVal").innerText = this.jsonObject["Pressure"].valueOf().toString().substr(0,5) + "Pa"
      document.getElementById("AltVal").innerText = this.jsonObject["Altitude"].valueOf().toString().substr(0,3) + "m"
      document.getElementById("DeviceVal").innerText = this.jsonObject["DeviceId"].valueOf().toString()
    }catch(e){
      if(e instanceof TypeError)      
      console.log("No connection to measurement device established")
      console.log("Exception" + e.name + ": " + e.message);
    }
  }

  // fetch telemetry data from backend for the current device
  fetchTelemetry() {
    return this.service.getTelemetryJson(this.bData$["device"]).subscribe(jsonObject => {
     this.jsonObject = jsonObject;
     console.log(this.jsonObject);
    });
  }
}
