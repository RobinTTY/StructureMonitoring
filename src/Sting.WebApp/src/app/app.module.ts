import { AppComponent } from './app.component';
import { RoomComponent } from './room/room.component';

import { AboutComponent } from './about/about.component';
import { FloorsComponent } from './floors/floors.component';
import { BuildingsComponent } from './buildings/buildings.component';
import { LineChartComponent } from './line-chart/line-chart.component';
import { FloorDetailComponent } from './floor-detail/floor-detail.component';
import { NavigationBarComponent } from './navigation-bar/navigation-bar.component';

import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';




@NgModule({
  declarations: [
    AppComponent,
    RoomComponent,
    AboutComponent,
    FloorsComponent,
    LineChartComponent,
    BuildingsComponent,
    FloorDetailComponent,
    NavigationBarComponent,
  ],
  imports: [
    NgbModule,
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
