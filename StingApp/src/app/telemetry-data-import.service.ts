import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TelemetryDataImportService {

  constructor(private http: HttpClient) { }
  
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
