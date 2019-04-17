import {Component, OnInit} from '@angular/core';
import {TelemetryDataImportService} from '../telemetry-data-import.service';
import {Router} from '@angular/router';

// Get building data from local file
import * as json1 from '../buildings';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-floor',
  templateUrl: './floor.component.html',
  styleUrls: ['./floor.component.scss']
})

// TODO: change horrible naming practice in building config!!!
export class FloorComponent implements OnInit {
  // TODO: variable naming
  urlSplit: Array<string>;
  telemetryData: any;
  buildingData: Object;
  floorData: any;

  constructor(private service: TelemetryDataImportService, private router: Router) {
  }

  ngOnInit() {
    this.fetchTelemetry();

    this.urlSplit = this.router.url.split('/');
    this.buildingData = json1.default.buildings[parseInt(this.urlSplit[2], 10) - 1].floors[parseInt(this.urlSplit[4], 10) - 1].rooms;
    this.floorData = json1.default.buildings[parseInt(this.urlSplit[2], 10) - 1].floors[parseInt(this.urlSplit[4], 10) - 1];
  }

  // TODO: research lifecycle hooks, use jquery?!
  ngAfterViewInit() {
    console.log('bdata:');
    console.log(this.buildingData);

    for (let i = 0; i < this.buildingData['length'].valueOf(); i++) {
      document.getElementById('txt' + (i + 1)).style.setProperty('left', this.buildingData[i]['x'].valueOf() + '%');
      document.getElementById('txt' + (i + 1)).style.setProperty('top', this.buildingData[i]['y'].valueOf() + '%');

      if (this.buildingData[i].device === 'default') {
        document.getElementById('txt' + (i + 1)).style.display = 'none';
      }
    }
  }

  // # Insert room status
  ngDoCheck() {
    for (let i = 0; i < this.buildingData['length'].valueOf(); i++) {
      let str = '';
      let dev_data = '';
      try {
        for (let j = 0; j < this.telemetryData['length'].valueOf(); j++) {
          if (this.telemetryData[j]['DeviceId'] === this.buildingData[i]['device']) {
            dev_data = this.telemetryData[j];
            break;
          }
        }

        // TODO: refactor
        // Status insertion based on thresholds of each individual room
        const thresholds =
          json1.default.buildings[parseInt(this.urlSplit[2], 10) - 1].floors[parseInt(this.urlSplit[4], 10) - 1].rooms[i].thresholds;
        let statusOK = true;

        if (dev_data['Temperature'].valueOf() >= thresholds['tempHigh']) {
          str += '🔥';
          statusOK = false;
        } else if (dev_data['Temperature'].valueOf() <= thresholds['tempLow']) {
          str += '❄';
          document.getElementById('txt' + (i + 1)).style.setProperty('color', 'blue');
          statusOK = false;
        }
        if (dev_data['Humidity'].valueOf() >= thresholds['humHigh']) {
          str += '💧';
          statusOK = false;
        } else if (dev_data['Humidity'].valueOf() <= thresholds['humLow']) {
          str += '🌵';
          document.getElementById('txt' + (i + 1)).style.setProperty('color', 'blue');
          statusOK = false;
        }
        if (statusOK) {
          str = '👍';
        }

        document.getElementById('txt' + (i + 1)).innerText = str;
      } catch (e) {
        if (e instanceof TypeError) {
          // ignore uninitialized data
        } else {
          console.log('Exception' + e.name + ': ' + e.message);
        }
      }
    }
  }

  fetchTelemetry() {
    // TODO: move
    const localTime = new Date();
    const utcTime = Date.UTC(localTime.getUTCFullYear(), localTime.getUTCMonth(), localTime.getUTCDate(),
    localTime.getUTCHours(), localTime.getUTCMinutes() - 2, localTime.getUTCSeconds());

    const params = new HttpParams()
    .set('DeviceId', 'RasPi_Robin')
    .set('TimeStampStart', utcTime.toString());

    // TODO: research Observables
    return this.service.getTelemetryJson(params).subscribe(jsonObject => {
      this.telemetryData = jsonObject;

      console.log('telemetry data:');
      console.log(this.telemetryData);
    });
  }
}
