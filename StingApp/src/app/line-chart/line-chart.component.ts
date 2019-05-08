import { Component, OnInit } from '@angular/core';

import { TelemetryData } from '../shared/models/telemetryData';

import { Chart } from 'chart.js';
import 'chartjs-plugin-annotation';

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.scss']
})
export class LineChartComponent implements OnInit {

  private chart: Chart;
  private telemetryData: TelemetryData;

  constructor() { }

  ngOnInit() {
    // insert chart data
    console.log('This got here:');
    console.log(this.telemetryData);

    const temperature = Object.values(this.telemetryData).map(val => val['temperature']._.substr(0, 5));
    const humidity = Object.values(this.telemetryData).map(val => val['humidity']._);
    // const altitude = Object.values(this.telemetryData).map(val => val.altitude._);
    const dateTime = Object.values(this.telemetryData).map(val => val['unixTimeStamp']._);

    const weatherDates = [];
    dateTime.forEach((val) => {
      const date = new Date(val * 1);
      weatherDates.push(date.toLocaleTimeString('en-EN', { month: 'short', day: 'numeric', hour: 'numeric', minute: 'numeric' }));
      // To show year, month or day, use the following parameters
      // weatherDates.push(date.toLocaleTimeString('de-DE', {year: 'numeric', month: 'short', day: 'numeric'}))
    });

    this.chart = new Chart('canvasTemp', {
      type: 'line',
      data: {
        labels: weatherDates,
        datasets: [
          {
            label: 'Temperature',
            data: temperature,
            borderColor: '#e21212',
            fill: false
          }
        ]
      },
      options: {
        responsive: true,
        elements: {
          point: {
            radius: 0,
            hitRadius: 10,
            hoverRadius: 10
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
              labelString: 'Last 24 hours'
            }
          }],
          yAxes: [{
            display: true,
            scaleLabel: {
              display: true,
              labelString: 'Temperature in °C'
            }
          }]
        },
        //   tooltips:{
        //     mode: 'index',
        //     intersect: true
        //   },

        //   annotation: {
        //     annotations: [{
        //       type: 'line',
        //       mode: 'horizontal',
        //       scaleID: 'y-axis-0',
        //       value: tempAverage,
        //       borderColor: '#3334C9',
        //       borderWidth: 3,
        //       label: {
        //         enabled: true,
        //         content: 'Low threshold'
        //       }
        //     }
        //   ]
        // }
      }
    });

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
            radius: 0,
            hitRadius: 10,
            hoverRadius: 10
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
              labelString: 'Last 24 hours'
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
    });
  }
}
