import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationBar } from './navigation-bar/navigation-bar.component';
import { AboutComponent } from './about/about.component';
import { RoomComponent } from './room/room.component';
import { FloorDetailComponent } from './floor-detail/floor-detail.component';
import { FloorsComponent } from './floors/floors.component';

import { HttpClientModule } from '@angular/common/http';
import { BuildingsComponent } from './buildings/buildings.component';
import { LineChartComponent } from './line-chart/line-chart.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationBar,
    AboutComponent,
    RoomComponent,
    FloorDetailComponent,
    FloorsComponent,
    BuildingsComponent,
    LineChartComponent
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
