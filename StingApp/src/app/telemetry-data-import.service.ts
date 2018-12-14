import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import  { map } from 'rxjs/operators';
import { Observable } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class TelemetryDataImportService {

  constructor(private http: HttpClient) { }
  
  getTelemetryJson(device) {
      return this.http.get('http://localhost:1337/telemetry/current/' + device);
  }
}
