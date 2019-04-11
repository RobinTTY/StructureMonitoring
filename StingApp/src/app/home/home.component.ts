import {Component, OnInit} from '@angular/core';
import {TelemetryDataImportService} from '../telemetry-data-import.service';

// Get building data from local file
import * as json1 from '../buildings';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  // TODO: variable naming
  bData$: Object;
  telemetry$: Object;
  public jsonObject: any;

  constructor(private telemetryService: TelemetryDataImportService) {
  }

  ngOnInit() {
    this.bData$ = json1.default.buildings;
  }
}
