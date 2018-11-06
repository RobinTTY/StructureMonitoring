import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-floor',
  templateUrl: './floor.component.html',
  styleUrls: ['./floor.component.scss']
})
export class FloorComponent implements OnInit {

  floor$: Object;

  constructor(private data: TelemetryDataImportService) {
  }

  ngOnInit() {
    this.data.getRooms().subscribe(data => this.floor$ = data)
  }

}