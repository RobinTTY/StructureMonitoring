import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TelemetryDataImportService {

  telemetryDataUrl = 'https://localhost:44306/api/TelemetryData';

  constructor(private http: HttpClient) {
  }

  public getTelemetryJson(device: string) {
    return this.http.get(this.telemetryDataUrl);
  }
}
