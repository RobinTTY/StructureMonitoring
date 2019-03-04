import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class TelemetryDataImportService {

  constructor(private http: HttpClient) {
  }

  public getTelemetryJson(device: string) {
    return this.http.get('https://backendsting.azurewebsites.net/telemetry/current/' + device);
  }

  public InvokeDeviceMethod(method: string, device: string) {
    return this.http.get('https://backendsting.azurewebsites.net/invoke/' + method + '/' + device);
  }
}
