import {Component, OnInit} from '@angular/core';
import {TelemetryDataImportService} from '../telemetry-data-import.service';

// Get building data from local file
import * as buildingConfig from '../buildings.json';

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
    this.bData$ = buildingConfig.buildings;
  }
}
