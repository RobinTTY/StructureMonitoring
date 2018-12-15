import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ChartService {

  constructor(private _http: HttpClient) { }

  deviceData(){
    return this._http.get("https://backendsting.azurewebsites.net/telemetry/lastweek/RasPi_Enes")
      .pipe(map(result => result));
  }
}
