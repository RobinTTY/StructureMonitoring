import { Injectable } from '@angular/core';
import { Building } from '../../shared/models/building';
import * as buildingConfig from '../../buildings.json';

@Injectable({
  providedIn: 'root'
})
export class ConfigProviderService {

  config: Array<Building>;

  constructor() {
    this.castConfig();
   }

  private castConfig() {
    this.config = <Array<Building>><any>buildingConfig.buildings;
  }

  getConfig(): Array<Building> {
    return this.config;
  }
}
