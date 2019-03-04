import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ChartService {

  constructor(private _http: HttpClient) {
  }

  deviceData(device: string) {
    return this._http.get('https://backendsting.azurewebsites.net/telemetry/lastday/' + device)
      .pipe(map(result => result));
  }
}
