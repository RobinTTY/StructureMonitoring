import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoomComponent } from './room/room.component';
import { FloorComponent } from './floor/floor.component';
import { BuildingComponent } from './building/building.component';
import { TelemetryDataComponent } from './telemetry-data/telemetry-data.component';

const routes: Routes = [
  {
    path: '',
    component: TelemetryDataComponent
  },
  {
    path: 'room/',
    component: RoomComponent
  },
  {
    path: 'floor/',
    component: FloorComponent
  },
  {
    path: 'building/',
    component: BuildingComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
