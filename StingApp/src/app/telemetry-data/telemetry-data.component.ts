import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-telemetry-data',
  templateUrl: './telemetry-data.component.html',
  styleUrls: ['./telemetry-data.component.scss']
})
export class TelemetryDataComponent implements OnInit {


  telemetry$: Object;

  constructor(private data: TelemetryDataImportService) { }

  ngOnInit() {
    this.data.getTelemetryData().subscribe(
      data => this.telemetry$ = data
    )
  }

}
