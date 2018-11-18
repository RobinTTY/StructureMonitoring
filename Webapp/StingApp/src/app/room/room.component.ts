import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Alert } from 'selenium-webdriver';

//Get building data from local file
import * as json1 from '../buildings';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})

export class RoomComponent implements OnInit {

  bData$: Object;
  room$: Object;

  constructor(private service: TelemetryDataImportService, private route: ActivatedRoute, private http: HttpClient) { 
    this.route.params.subscribe(params => this.room$ = params.id)
  }

  ngOnInit() {
    this.bData$ = json1.default;
    this.room$ = this.service.getRoom(this.room$).subscribe(data => this.room$ = data)
  }

}
