import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {forkJoin} from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class TelemetryDataImportService {

  constructor(private http: HttpClient) { }
  
  public getTelemetryJson(device : string) {
      return this.http.get('http://localhost:1337/telemetry/current/' + device);
  }
}
