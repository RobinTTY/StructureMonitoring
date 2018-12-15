import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ChartService {

  constructor(private _http: HttpClient) { }

  deviceEnesData(){
    return this._http.get("http://localhost:1337/telemetry/lastmonth/RasPi_Robin")
      .pipe(map(result => result));
  }
}
