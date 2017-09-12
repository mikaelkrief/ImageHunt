import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';

import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HomeModule } from "../home/home.module";
import { GameModule } from "../game/game.module";
import { PageNotFoundModule } from "../page-not-found/page.not.found.module";
import { TeamModule } from "../team/team.module";
import { TeamComponent } from "../team/team.component";
import { HomeComponent } from "../home/home.component";
import { GameComponent } from "../game/game.component";
import { PageNotFoundComponent } from "../page-not-found/page.not.found.component";
import { NavmenuModule } from "../navmenu/navmenu.module";
import { MapModule } from "../map/map.module";
import { MapComponent } from "../map/map.component";
import {NewAdminModule} from "../admin/new-admin/new.admin.module";
import {GameDetailModule} from "../game-detail/game.detail.module";
import {GameDetailComponent} from "../game-detail/game.detail.component";
import {AdminListModule} from "../admin/admin-list/admin-list.module";
import {AdminListComponent} from "../admin/admin-list/admin-list.component";

@NgModule({
  declarations: [
    AppComponent,
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    HomeModule,
    GameModule,
    GameDetailModule,
    TeamModule,
    MapModule,
    AdminListModule,
    NewAdminModule,
    NavmenuModule,
    PageNotFoundModule,
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent },
      { path: 'game', component: GameComponent },
      { path: 'game/:gameId', component: GameDetailComponent },
      { path: 'team', component: TeamComponent },
      { path: 'map', component: MapComponent },
      { path: 'admin', component: AdminListComponent },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: '**', component: PageNotFoundComponent }])
  ],
  providers: []
})
export class AppModule { } 
