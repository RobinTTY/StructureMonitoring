import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-building',
  templateUrl: './building.component.html',
  styleUrls: ['./building.component.scss']
})
export class BuildingComponent implements OnInit {

  building$: Object;
  roomData$: Object;

  constructor(private data: TelemetryDataImportService) {
  }

  ngOnInit() {
    this.data.getFloors().subscribe(data => this.building$ = data)
    this.data.getRoom(1).subscribe(data => this.roomData$ = data)
  }

}
