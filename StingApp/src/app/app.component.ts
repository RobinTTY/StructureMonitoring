import { Component } from '@angular/core';
import {ChartService} from './chart.service';
import {Chart} from 'chart.js'
import {map} from 'rxjs/operators';
import { analyzeAndValidateNgModules } from '@angular/compiler';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {
  title = 'StingApp';

  chart = [];


  constructor (private _chart: ChartService){}

  ngOnInit(){
    this._chart.deviceEnesData()
      .subscribe(res => {
        
        let temperature = Object.values(res).map(res => res.temperature._);
        let humidity = Object.values(res).map(res => res.humidity._);
        let altitude = Object.values(res).map(res => res.altitude._);
        let dateTime = Object.values(res).map(res => res.unixtime._);

        let weatherDates = []
        dateTime.forEach((res) => {
          let jsdate = new Date (res * 1)
          weatherDates.push(jsdate.toLocaleTimeString('de-DE'))
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
            legend: {
              display: false
            },
            scales: {
              xAxes: [{
                display: true
              }],
              yAxes: [{
                display: true
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
            legend: {
              display: false
            },
            scales: {
              xAxes: [{
                display: true
              }],
              yAxes: [{
                display: true
              }]
            }
          }
        })

      })
  }

}
