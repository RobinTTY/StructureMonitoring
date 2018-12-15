import { Component, OnInit } from '@angular/core';
import { TelemetryDataImportService} from '../telemetry-data-import.service';
import { Router } from '@angular/router'
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

//Get building data from local file
import * as json1 from '../buildings';

@Component({
  selector: 'app-floor',
  templateUrl: './floor.component.html',
  styleUrls: ['./floor.component.scss']
})

export class FloorComponent implements OnInit {

  urlSplit$: Array<string>;
  public jsonObject: any;
  bData$: Object;
  floor$: Object;

  constructor(private service: TelemetryDataImportService, private router: Router) {
  }

  ngOnInit() {    
    this.fetchTelemetry();
    this.urlSplit$ = this.router.url.split('/')
    this.bData$ = json1.default.buildings[parseInt(this.urlSplit$[2]) - 1].floors[parseInt(this.urlSplit$[4]) - 1].rooms;
    this.floor$ = json1.default.buildings[parseInt(this.urlSplit$[2]) - 1].floors[parseInt(this.urlSplit$[4]) - 1];
    for(let i = 0; i < this.bData$["length"].valueOf(); i++) {
      document.getElementById("txt" + (i + 1)).style.setProperty('left', this.bData$[i]["x"].valueOf() + '%');
      document.getElementById("txt" + (i + 1)).style.setProperty('top', this.bData$[i]["y"].valueOf() + '%');
      if(this.bData$[i].device == "default")       
        document.getElementById("txt" + (i + 1)).style.display = "none";
    }    
  }

  // # Insert room status
  ngDoCheck() {
    for(let i = 0; i < this.bData$["length"].valueOf(); i++) {
      let str = "";      
      try {
        if(this.jsonObject["Temperature"].valueOf() >= 30.7){
          str = "🙁";
        } 
        else {
          str = "🙂";          
        }      
      document.getElementById("txt" + (0 + 1)).innerText = str;
      }
      catch(TypeError){
      console.log("No connection to measurement device established")
      }
    }
  }

  fetchTelemetry() {
    return this.service.getTelemetryJson("all").subscribe(jsonObject => {
     this.jsonObject = jsonObject;
     console.log(this.jsonObject);
    });
  }
}
