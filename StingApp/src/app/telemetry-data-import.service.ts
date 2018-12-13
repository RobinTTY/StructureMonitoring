import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import  { map } from 'rxjs/operators';
import { Observable } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class TelemetryDataImportService {

  constructor(private http: HttpClient) { }
  
  getTelemetryJson() {
      return this.http.get('http://localhost:1337/telemetry/current/RasPi_Robin');
  }

  getRoom(num){
    return this.http.get('https://jsonplaceholder.typicode.com/users/'+num)
  }

  //telemetry endpoint als url eingeben!
  getBuildings(){
    return this.http.get('https://jsonplaceholder.typicode.com/users')
  }

  getFloors(){
    return this.http.get('https://jsonplaceholder.typicode.com/users')
  }

  getRooms(){
    return this.http.get('https://jsonplaceholder.typicode.com/users')
  }
}
