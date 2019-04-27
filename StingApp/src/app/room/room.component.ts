import { Component, OnInit } from '@angular/core';
import { TelemetryService } from '../services/telemetry/telemetry.service';
import { TelemetryData } from '../shared/models/TelemetryData';
import { ActivatedRoute } from '@angular/router';
import { HttpParams } from '@angular/common/http';
import { Position } from '../shared/models/position';
import 'chartjs-plugin-annotation';

// Get building data from local file
import * as buildingConfig from '../buildings.json';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})

export class RoomComponent implements OnInit {

  // TODO: variable naming! This component needs the line-chart, refer to html
  private telemetryData: TelemetryData;
  private roomData: any;
  private thresholds: Array<number>;
  private position: Position;

  constructor(private telemetryService: TelemetryService, private route: ActivatedRoute) {

  }

  // get configuration data for current room, fetch telemetry data
  ngOnInit() {
    const urlParams = this.route.snapshot.paramMap;
    this.position = new Position(+urlParams.get('buildingId'), +urlParams.get('floorId'), +urlParams.get('roomId'));

    this.roomData = buildingConfig
      .buildings[this.position.buildingId]
      .floors[this.position.floorId]
      .rooms[this.position.roomId];
    this.thresholds = this.roomData['thresholds'];

    this.fetchTelemetry();

    // TODO: remove diagnostics
    console.log(this.position);
    console.log(this.thresholds);
    console.log(this.roomData);
  }

  // TODO: refactor, 
  loadTelemetry() {
    console.log(this.telemetryData);
    const dt = new Date(this.telemetryData.timeStamp)
      //.toLocaleTimeString('en-EN', { weekday: 'long', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric' });


    console.log(this.telemetryData.timeStamp);
    document.getElementById('TempVal').innerText = this.telemetryData.temperature.toString().substr(0, 5) + '°C';
    document.getElementById('HumVal').innerText = this.telemetryData.humidity.toString() + '%';
    document.getElementById('PressVal').innerText = this.telemetryData.airPressure.toString().substr(0, 3) + 'hPa';
    // document.getElementById('AltVal').innerText = this.telemetryData['Altitude'].valueOf().toString().substr(0, 3) + 'm';
    document.getElementById('DeviceVal').innerText = this.telemetryData.deviceId.toString();
    document.getElementById('TimeVal').innerText = dt.toString();

    if (this.telemetryData.temperature >= this.thresholds['tempHigh']) {
      document.getElementById('temperature').innerText = 'above threshold of ' + this.thresholds['tempHigh'] + '°C';
      document.getElementById('temperature-body').style.backgroundColor = 'rgba(259, 67, 95, 0.35)';
    } else if (this.telemetryData.temperature <= this.thresholds['tempLow']) {
      document.getElementById('temperature').innerText = 'below threshold of ' + this.thresholds['tempLow'] + '°C';
      document.getElementById('temperature-body').style.backgroundColor = 'rgba(259, 67, 95, 0.35)';
    }
    if (this.telemetryData.humidity >= this.thresholds['humHigh']) {
      document.getElementById('humidity').innerText = 'above threshold of ' + this.thresholds['humHigh'] + '%';
      document.getElementById('humidity-body').style.backgroundColor = 'rgba(259, 67, 95, 0.35)';
    } else if (this.telemetryData.humidity <= this.thresholds['humLow']) {
      document.getElementById('humidity').innerText = 'below threshold of ' + this.thresholds['humLow'] + '%';
      document.getElementById('humidity-body').style.backgroundColor = 'rgba(259, 67, 95, 0.35)';
    }
  }

  // fetch telemetry data from backend for the current device
  fetchTelemetry() {
    const localTime = new Date();
    const utcTime = Date.UTC(localTime.getUTCFullYear(), localTime.getUTCMonth(), localTime.getUTCDate(),
      localTime.getUTCHours(), localTime.getUTCMinutes() - 10, localTime.getUTCSeconds());

    const params = new HttpParams()
      .set('DeviceId', 'RasPi_Robin')
      .set('TimeStampStart', utcTime.toString());

    return this.telemetryService.getTelemetryJson(params).subscribe(result => {
      this.telemetryData = result[0];
      this.loadTelemetry();
      console.log('Telemetry Data:');
      console.log(this.telemetryData);
    });
  }

  // TODO: might want to implement this wit AWS lambda?!
  // if device card is clicked call device method
  // locate() {
  //   this.service.InvokeDeviceMethod('Locate', this.bData$['device']).subscribe();
  // }
}
