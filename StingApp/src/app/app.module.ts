import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {SidebarComponent} from './sidebar/sidebar.component';
import {UsersComponent} from './users/users.component';
import {RoomComponent} from './room/room.component';
import {FloorComponent} from './floor/floor.component';
import {BuildingComponent} from './building/building.component';

import {HttpClientModule} from '@angular/common/http';
import {HomeComponent} from './home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent,
    UsersComponent,
    RoomComponent,
    FloorComponent,
    BuildingComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
