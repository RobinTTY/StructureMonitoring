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
    // TODO: insert new Backend API
    return this._http.get('')
      .pipe(map(result => result));
  }
}
