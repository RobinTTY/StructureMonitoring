import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { HttpParams } from '@angular/common/http';

import { Room } from '../shared/models/room';
import { Building } from '../shared/models/building';
import { Position } from '../shared/models/position';
import { TelemetryData } from '../shared/models/telemetryData';

import { TelemetryService } from '../services/telemetry/telemetry.service';
import { ConfigProviderService } from '../services/configProvider/config-provider.service';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})

// TODO: probably get data for charts here and insert trough html, refer to tutorial
export class RoomComponent implements OnInit {

  // TODO: This component needs the line-chart, refer to html
  private telemetryData: TelemetryData;
  private buildings: Array<Building>;
  public roomData: Room;
  public floorAlias: string;
  public position: Position;

  constructor(private telemetryService: TelemetryService,
              private configService: ConfigProviderService,
              private routeService: ActivatedRoute) { }

  // get configuration data for current room, fetch telemetry data
  public ngOnInit(): void {
    const urlParams = this.routeService.snapshot.paramMap;
    this.position = new Position(+urlParams.get('buildingId'), +urlParams.get('floorId'), +urlParams.get('roomId'));
    this.buildings = this.configService.getBuildingConfig();
    this.roomData = this.buildings[this.position.buildingId].floors[this.position.floorId].rooms[this.position.roomId];
    this.floorAlias = this.buildings[this.position.buildingId].floors[this.position.floorId].alias;
    this.fetchTelemetry();
  }

  // TODO: refactor
  private loadTelemetry(): void {
    const dt = new Date(this.telemetryData.timeStamp);
      // .toLocaleTimeString('en-EN', { weekday: 'long', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric' });

    document.getElementById('TempVal').innerText = this.telemetryData.temperature.toString().substr(0, 5) + '°C';
    document.getElementById('HumVal').innerText = this.telemetryData.humidity.toString() + '%';
    document.getElementById('PressVal').innerText = this.telemetryData.airPressure.toString().substr(0, 3) + 'hPa';
    // document.getElementById('AltVal').innerText = this.telemetryData['Altitude'].valueOf().toString().substr(0, 3) + 'm';
    document.getElementById('DeviceVal').innerText = this.telemetryData.deviceId.toString();
    document.getElementById('TimeVal').innerText = dt.toString();

    if (this.telemetryData.temperature >= this.roomData.thresholds.tempHigh) {
      document.getElementById('temperature').innerText = 'above threshold of ' + this.roomData.thresholds.tempHigh + '°C';
      document.getElementById('temperature-body').style.backgroundColor = 'rgba(259, 67, 95, 0.35)';
    } else if (this.telemetryData.temperature <= this.roomData.thresholds.tempLow) {
      document.getElementById('temperature').innerText = 'below threshold of ' + this.roomData.thresholds.tempLow + '°C';
      document.getElementById('temperature-body').style.backgroundColor = 'rgba(259, 67, 95, 0.35)';
    }
    if (this.telemetryData.humidity >= this.roomData.thresholds.humHigh) {
      document.getElementById('humidity').innerText = 'above threshold of ' + this.roomData.thresholds.humHigh + '%';
      document.getElementById('humidity-body').style.backgroundColor = 'rgba(259, 67, 95, 0.35)';
    } else if (this.telemetryData.humidity <= this.roomData.thresholds.humLow) {
      document.getElementById('humidity').innerText = 'below threshold of ' + this.roomData.thresholds.humLow + '%';
      document.getElementById('humidity-body').style.backgroundColor = 'rgba(259, 67, 95, 0.35)';
    }
  }

  // fetch telemetry data from backend for the current device
  private fetchTelemetry(): void {
    const localTime = new Date();
    const utcTime = Date.UTC(localTime.getUTCFullYear(), localTime.getUTCMonth(), localTime.getUTCDate(),
      localTime.getUTCHours(), localTime.getUTCMinutes() - 10, localTime.getUTCSeconds());

    const params = new HttpParams()
      .set('DeviceId', 'RasPi_Robin')
      .set('TimeStampStart', utcTime.toString());

    this.telemetryService.getTelemetryJson(params).subscribe(result => {
      this.telemetryData = result[0];
      this.loadTelemetry();
      console.log('Telemetry Data:');
      console.log(this.telemetryData);
    });
  }
}
