import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TelemetryDataImportService {

  constructor(private http: HttpClient) { }
  //telemetry endpoint als url eingeben!
  getTelemetryData(){
    return this.http.get('https://jsonplaceholder.typicode.com/users')
  }

}
