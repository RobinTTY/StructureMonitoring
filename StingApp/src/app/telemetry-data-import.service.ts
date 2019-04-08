import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class TelemetryDataImportService {

  constructor(private http: HttpClient) {
  }

  public getTelemetryJson(device: string) {
    // TODO: insert new Backend API
    return this.http.get('https://localhost:44306/api/TelemetryData');
  }

  public InvokeDeviceMethod(method: string, device: string) {
    return this.http.get('https://localhost:44306/api/TelemetryData');
  }
}
