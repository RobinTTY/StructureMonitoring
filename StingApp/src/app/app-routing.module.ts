import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoomComponent } from './room/room.component';
import { FloorComponent } from './floor/floor.component';
import { BuildingComponent } from './building/building.component';
import { HomeComponent } from './home/home.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'users',
    component: UsersComponent
  },
  {
    path: 'building/:buildingId/floor/:floorId/room/:roomId',
    component: RoomComponent
  },
  {
    path: 'building/:buildingId/floor/:floorId',
    component: FloorComponent
  },
  {
    path: 'building/:buildingId',
    component: BuildingComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
