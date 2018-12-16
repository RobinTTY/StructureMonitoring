import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoomComponent } from './room/room.component';
import { FloorComponent } from './floor/floor.component';
import { BuildingComponent } from './building/building.component';
import { TelemetryDataComponent } from './telemetry-data/telemetry-data.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
  {
    path: '',
    component: TelemetryDataComponent
  },
  {
    path: 'users',
    component: UsersComponent
  },
  {
    path: 'room/:id',
    component: RoomComponent
  },
  {
    path: 'building/:id/floor/:id/room/:id',
    component: RoomComponent
  },
  {
    path: 'building/:id/floor/:id',
    component: FloorComponent
  },
  {
    path: 'building/:id',
    component: BuildingComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
