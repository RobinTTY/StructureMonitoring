import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { Floor } from '../shared/models/floor';
import { Position } from '../shared/models/position';

import { ConfigProviderService } from '../services/configProvider/config-provider.service';

@Component({
  selector: 'app-floors',
  templateUrl: './floors.component.html',
  styleUrls: ['./floors.component.scss']
})

export class FloorsComponent implements OnInit {

  public floors: Array<Floor>;

  constructor(private routeService: ActivatedRoute, private configService: ConfigProviderService) {
  }

  public ngOnInit(): void {
    const urlParams = this.routeService.snapshot.paramMap;
    const position = new Position(+urlParams.get('buildingId'), +urlParams.get('floorId'));

    const buildings = this.configService.getBuildingConfig();
    this.floors = buildings[position.buildingId].floors;
  }
}
