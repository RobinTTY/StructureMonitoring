import { Component, OnInit } from '@angular/core';

import { Building } from '../shared/models/building';

import { ConfigProviderService } from '../services/configProvider/config-provider.service';

@Component({
  selector: 'app-buildings',
  templateUrl: './buildings.component.html',
  styleUrls: ['./buildings.component.scss']
})
export class BuildingsComponent implements OnInit {
  public buildings: Array<Building>;

  constructor(private configService: ConfigProviderService) {
  }

  public ngOnInit(): void {
    this.buildings = this.configService.getBuildingConfig();
  }
}
