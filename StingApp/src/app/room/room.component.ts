import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Observable } from 'rxjs';
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

  constructor(private service: TelemetryDataImportService, private route: ActivatedRoute, private http: HttpClient, private router: Router) { 
    this.route.params.subscribe(params => this.room$ = params.id)
  }

  ngOnInit() {
    this.fetchTelemetry();
    this.urlSplit$ = this.router.url.split('/')
    this.bData$ = json1.default.buildings[parseInt(this.urlSplit$[2]) - 1].floors[parseInt(this.urlSplit$[4]) - 1].rooms[parseInt(this.urlSplit$[6]) - 1];
    this.room$ = this.service.getRoom(this.room$).subscribe(data => this.room$ = data)
  }

  ngDoCheck() {
    document.getElementById("TempVal").innerText = this.jsonObject["Temperature"].valueOf().toString().substr(0,5) + "°C"
    document.getElementById("HumVal").innerText = this.jsonObject["Humidity"].valueOf().toString() + "%"
    document.getElementById("PressVal").innerText = this.jsonObject["Pressure"].valueOf().toString().substr(0,5) + "Pa"
    document.getElementById("AltVal").innerText = this.jsonObject["Altitude"].valueOf().toString().substr(0,3) + "m"
    document.getElementById("DeviceVal").innerText = this.jsonObject["DeviceId"].valueOf().toString()
  }

  fetchTelemetry() {
    return this.service.getTelemetryJson().subscribe(jsonObject => {
     this.jsonObject = jsonObject;
     console.log(this.jsonObject);
    });
  }
}
