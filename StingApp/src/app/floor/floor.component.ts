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

export class FloorComponent implements OnInit {
  // TODO: variable naming
  urlSplit$: Array<string>;
  public jsonObject: any;
  bData$: Object;
  floor$: any;

  constructor(private service: TelemetryDataImportService, private router: Router) {
  }

  ngOnInit() {
    const telemetry = this.fetchTelemetry();
    console.log(telemetry);
    this.urlSplit$ = this.router.url.split('/');
    this.bData$ = json1.default.buildings[parseInt(this.urlSplit$[2]) - 1].floors[parseInt(this.urlSplit$[4]) - 1].rooms;
    this.floor$ = json1.default.buildings[parseInt(this.urlSplit$[2]) - 1].floors[parseInt(this.urlSplit$[4]) - 1];
  }

  // TODO: research lifecycle hooks
  ngAfterViewInit() {
    for (let i = 0; i < this.bData$['length'].valueOf(); i++) {
      document.getElementById('txt' + (i + 1)).style.setProperty('left', this.bData$[i]['x'].valueOf() + '%');
      document.getElementById('txt' + (i + 1)).style.setProperty('top', this.bData$[i]['y'].valueOf() + '%');
      if (this.bData$[i].device === 'default') {
        document.getElementById('txt' + (i + 1)).style.display = 'none';
      }
    }
  }

  // # Insert room status
  ngDoCheck() {
    for (let i = 0; i < this.bData$['length'].valueOf(); i++) {
      let str = '';
      let dev_data = '';
      try {
        for (let j = 0; j < this.jsonObject['length'].valueOf(); j++) {
          if (this.jsonObject[j]['DeviceId'] === this.bData$[i]['device']) {
            dev_data = this.jsonObject[j];
            break;
          }
        }

        // TODO: refactor
        // Status insertion based on thresholds of each individual room
        const thresholds =
          json1.default.buildings[parseInt(this.urlSplit$[2], 10) - 1].floors[parseInt(this.urlSplit$[4], 10) - 1].rooms[i].thresholds;
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

    return this.service.getTelemetryJson(params).subscribe(jsonObject => {
      this.jsonObject = jsonObject;
      console.log(this.jsonObject);
    });
  }
}
