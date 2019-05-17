import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoomComponent } from './room/room.component';
import { FloorDetailComponent } from './floor-detail/floor-detail.component';
import { FloorsComponent } from './floors/floors.component';
import { BuildingsComponent } from './buildings/buildings.component';
import { AboutComponent } from './about/about.component';

const routes: Routes = [
  {
    path: '',
    component: BuildingsComponent
  },
  {
    path: 'about',
    component: AboutComponent
  },
  {
    path: 'building/:buildingId/floor/:floorId/room/:roomId',
    component: RoomComponent
  },
  {
    path: 'building/:buildingId/floor/:floorId',
    component: FloorDetailComponent
  },
  {
    path: 'building/:buildingId',
    component: FloorsComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
