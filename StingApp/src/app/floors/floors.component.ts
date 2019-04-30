import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Position } from '../shared/models/position';
import { Floor } from '../shared/models/floor';

import { ConfigProviderService } from '../services/configProvider/config-provider.service';

@Component({
  selector: 'app-floors',
  templateUrl: './floors.component.html',
  styleUrls: ['./floors.component.scss']
})

export class FloorsComponent implements OnInit {

  private floors: Array<Floor>;

  constructor(private routeService: ActivatedRoute, private configService: ConfigProviderService) {
  }

  public ngOnInit(): void {
    const urlParams = this.routeService.snapshot.paramMap;
    let position = new Position(+urlParams.get('buildingId'), +urlParams.get('floorId'));

    var buildings = this.configService.getBuildingConfig();
    this.floors = buildings[position.buildingId].floors;
  }
}
