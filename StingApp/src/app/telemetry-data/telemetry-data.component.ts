import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Observable } from 'rxjs';


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
  

 

}
