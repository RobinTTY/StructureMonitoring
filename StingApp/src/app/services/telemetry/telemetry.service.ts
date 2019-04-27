import {TelemetryData} from '../../shared/models/TelemetryData';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TelemetryService {

  telemetryDataUrl = 'https://localhost:44306/api/TelemetryData/';

  constructor(private http: HttpClient) {
  }

  public getTelemetryJson(searchParams?: HttpParams): Observable<TelemetryData[]> {
    return this.http.get<TelemetryData[]>(this.telemetryDataUrl, { params: searchParams });
  }
}
