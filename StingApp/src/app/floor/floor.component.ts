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

  constructor(private data: TelemetryDataImportService, private router: Router) {
  }

  ngOnInit() {
    this.fetchTelemetry()
    this.urlSplit$ = this.router.url.split('/')
    this.bData$ = json1.default.buildings[parseInt(this.urlSplit$[2]) - 1].floors[parseInt(this.urlSplit$[4]) - 1].rooms;
    this.floor$ = json1.default.buildings[parseInt(this.urlSplit$[2]) - 1].floors[parseInt(this.urlSplit$[4]) - 1];
    for(let i = 0; i < this.bData$["length"].valueOf(); i++) {
      document.getElementById("txt" + (i + 1)).style.setProperty('left', this.bData$[i]["x"].valueOf() + '%');
      document.getElementById("txt" + (i + 1)).style.setProperty('top', this.bData$[i]["y"].valueOf() + '%');
      document.getElementById("txt" + (i + 1)).style.setProperty('opacity', '1');
    }
  }

  // #BesteAmpel
  ngDoCheck() {
    for(let i = 0; i < this.bData$["length"].valueOf(); i++) {
      let str = "0";
      if(this.jsonObject["Temperature"].valueOf() >= 18.7){
        str = "🙁";
      } else {
        str = "🙂";
      }
      document.getElementById("txt" + (i + 1)).innerText = str;
    }
  }

  fetchTelemetry() {
    return this.data.getTelemetryJson().subscribe(jsonObject => {
     this.jsonObject = jsonObject;
     console.log(this.jsonObject);
    });
  }
}
