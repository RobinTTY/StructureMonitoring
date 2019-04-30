import { TelemetryData } from '../../shared/models/TelemetryData';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TelemetryService {

  private telemetryDataUrl = 'https://localhost:44306/api/TelemetryData/';

  constructor(private http: HttpClient) {
  }

  // TODO: Error Handler, HttpParams from room!!!
  public getTelemetryJson(searchParams?: HttpParams): Observable<TelemetryData[]> {

    return this.http.get<TelemetryData[]>(this.telemetryDataUrl, { params: searchParams });
  }
}

export class TelemetryParams {
  constructor(deviceId?: string, timeStampStart?: number, timeStampStop?: number) {
    this.timeStampStart = Math.round(timeStampStart);
    this.timeStampStop = Math.round(timeStampStop);
    this.deviceId = deviceId;
  }

  deviceId: string;
  timeStampStart: number;
  timeStampStop: number;
}
