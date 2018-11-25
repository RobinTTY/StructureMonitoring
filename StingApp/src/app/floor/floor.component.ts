import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Router } from '@angular/router'
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

//Get building data from local file
import * as json1 from '../buildings';

@Component({
  selector: 'app-floor',
  templateUrl: './floor.component.html',
  styleUrls: ['./floor.component.scss']
})

export class FloorComponent implements OnInit {

  urlSplit$: Array<string>;
  bData$: Object;
  floor$: Object;

  constructor(private data: TelemetryDataImportService, private router: Router) {
  }

  ngOnInit() {
    this.urlSplit$ = this.router.url.split('/')
    this.bData$ = json1.default.buildings[parseInt(this.urlSplit$[2]) - 1].floors[parseInt(this.urlSplit$[4]) - 1].rooms;
    this.data.getRooms().subscribe(data => this.floor$ = data)
  }

}
