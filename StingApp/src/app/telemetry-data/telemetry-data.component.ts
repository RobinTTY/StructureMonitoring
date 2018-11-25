import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Observable } from 'rxjs';

//Get building data from local file
import * as json1 from '../buildings';

@Component({
  selector: 'app-telemetry-data',
  templateUrl: './telemetry-data.component.html',
  styleUrls: ['./telemetry-data.component.scss']
})
export class TelemetryDataComponent implements OnInit {

  public jsonObject: any;
  

  constructor(private telemetryService: TelemetryDataImportService) { }

  ngOnInit() {
   this.fetchTelemetry();
  }

  
  fetchTelemetry() {
    return this.telemetryService.getTelemetryJson().subscribe(jsonObject => {
     this.jsonObject = jsonObject;
      console.log(this.jsonObject);

    });
 
  }

export class TelemetryDataComponent implements OnInit {

  bData$: Object;
  telemetry$: Object;

  constructor(private data: TelemetryDataImportService) { }

  ngOnInit() {
    this.bData$ = json1.default.buildings;
    this.data.getBuildings().subscribe(data => this.telemetry$ = data)
  }
}
