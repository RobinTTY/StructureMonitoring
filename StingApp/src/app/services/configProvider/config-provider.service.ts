import { Injectable } from '@angular/core';
import { Building } from '../../shared/models/building';

import * as buildingConfig from '../../buildings.json';

@Injectable({
  providedIn: 'root'
})
export class ConfigProviderService {

  private config: Array<Building>;

  constructor() {
    this.castBuildingsConfig();    
  }

  private castBuildingsConfig(): void {
    this.config = <Array<Building>><any>buildingConfig.buildings;
  }

  public getBuildingConfig(): Array<Building> {
    return this.config;
  }
}
