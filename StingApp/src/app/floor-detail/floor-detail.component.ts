import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { HttpParams } from '@angular/common/http';

import { Room } from '../shared/models/room';
import { Floor } from '../shared/models/floor';
import { Position } from '../shared/models/position';
import { TelemetryData } from '../shared/models/telemetryData';

import { TelemetryService } from '../services/telemetry/telemetry.service';
import { ConfigProviderService } from '../services/configProvider/config-provider.service';

@Component({
  selector: 'app-floor-detail',
  templateUrl: './floor-detail.component.html',
  styleUrls: ['./floor-detail.component.scss']
})

// TODO: change horrible naming practice in building config!!!
export class FloorDetailComponent implements OnInit {
  private telemetryData: Array<TelemetryData>;
  private position: Position;
  private rooms: Array<Room>;
  private floorData: Floor;

  constructor(private telemetryService: TelemetryService,
              private configService: ConfigProviderService,
              private route: ActivatedRoute) {
  }

  public ngOnInit(): void {
    this.fetchTelemetry();
    const urlParams = this.route.snapshot.paramMap;
    this.position = new Position(+urlParams.get('buildingId'), +urlParams.get('floorId'));

    this.floorData = this.configService.getBuildingConfig()[this.position.buildingId].floors[this.position.floorId];
    this.rooms = this.floorData.rooms;
  }

  // TODO: research lifecycle hooks, use jquery -> maybe avoid this kind of editing somehow?!
  public ngAfterViewInit(): void {
    console.log('rooms:');
    console.log(this.rooms);

    for (let i = 0; i < this.rooms.length; i++) {
      document.getElementById(`txt${i}`).style.setProperty('left', this.rooms[i].x + '%');
      document.getElementById(`txt${i}`).style.setProperty('top', this.rooms[i].y + '%');

      if (this.rooms[i].device === 'default') {
        document.getElementById('txt' + (i + 1)).style.display = 'none';
      }
    }
  }

  // # Insert room status
  // TODO: why ngDoCheck
  public ngDoCheck(): void {
    for (let i = 0; i < this.rooms.length; i++) {
      let str = '';
      try {
        for (let j = 0; j < this.telemetryData.length; j++) {
          if (this.telemetryData[j].deviceId === this.rooms[i].device) {
            var deviceData = this.telemetryData[j];
            break;
          }
        }

        // TODO: refactor
        // Status insertion based on thresholds of each individual room
        const thresholds = this.rooms[i].thresholds;
        let statusOK = true;

        if (deviceData.temperature >= thresholds.tempHigh) {
          str += '🔥';
          statusOK = false;
        } else if (deviceData.temperature <= thresholds.tempLow) {
          str += '❄';
          document.getElementById('txt' + (i + 1)).style.setProperty('color', 'blue');
          statusOK = false;
        }
        if (deviceData.humidity >= thresholds.humHigh) {
          str += '💧';
          statusOK = false;
        } else if (deviceData.humidity <= thresholds.humLow) {
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

  private fetchTelemetry(): void {
    // TODO: move
    const localTime = new Date();
    const utcTime = Date.UTC(localTime.getUTCFullYear(), localTime.getUTCMonth(), localTime.getUTCDate(),
      localTime.getUTCHours() - 72, localTime.getUTCMinutes() - 2, localTime.getUTCSeconds());

    // this needs to get the last submitted value for each device
    const params = new HttpParams()
      .set('TimeStampStart', utcTime.toString());

    // TODO: research Observables
    this.telemetryService.getTelemetryJson(params).subscribe(result => {
      this.telemetryData = result;

      console.log('telemetry data:');
      console.log(this.telemetryData);
    });
  }
}
