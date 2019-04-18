import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

// Get building data from local file
import * as buildingConfig from '../buildings.json';

@Component({
  selector: 'app-building',
  templateUrl: './building.component.html',
  styleUrls: ['./building.component.scss']
})

export class BuildingComponent implements OnInit {

  // TODO: variable naming!
  urlSplit$: Array<string>;
  bData$: Object;
  building$: Object;

  constructor(private router: Router) {
  }

  ngOnInit() {
    this.urlSplit$ = this.router.url.split('/');
    this.bData$ = buildingConfig.buildings[parseInt(this.urlSplit$[2], 10) - 1].floors;
  }
}
