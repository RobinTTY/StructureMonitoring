import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

// Get building data from local file
import * as json1 from '../buildings';

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
    this.bData$ = json1.default.buildings[parseInt(this.urlSplit$[2], 10) - 1].floors;
  }
}
