import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';

import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { Ng2UiAuthModule, CustomConfig } from 'ng2-ui-auth';
import { HomeModule } from "../home/home.module";
import { PageNotFoundModule } from "../page-not-found/page.not.found.module";
import { TeamModule } from "../team/team.module";
import { TeamComponent } from "../team/team.component";
import { HomeComponent } from "../home/home.component";
import { PageNotFoundComponent } from "../page-not-found/page.not.found.component";
import { NavmenuModule } from "../navmenu/navmenu.module";
import { MapModule } from "../map/map.module";
import { MapComponent } from "../map/map.component";
import {NewAdminModule} from "../admin/new-admin/new.admin.module";
import {AdminListModule} from "../admin/admin-list/admin-list.module";
import {AdminListComponent} from "../admin/admin-list/admin-list.component";
import {GameListModule} from "../game/game-list/game-list.module";
import {GameDetailModule} from "../game/game-detail/game.detail.module";
import {GameListComponent} from "../game/game-list/game-list.component";
import {GameDetailComponent} from "../game/game-detail/game.detail.component";

export const GOOGLE_CLIENT_ID = '663482349038-jregj38js8hr7fm104jpnoisrgegorh8.apps.googleusercontent.com';
export class MyAuthConfig extends CustomConfig {
  defaultHeaders = { 'Content-Type': 'application/json' };
  providers = { google: { clientId: GOOGLE_CLIENT_ID } };
}
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
    GameListModule,
    GameDetailModule,
    TeamModule,
    MapModule,
    AdminListModule,
    NewAdminModule,
    NavmenuModule,
    PageNotFoundModule,
    Ng2UiAuthModule.forRoot(MyAuthConfig),
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent },
      { path: 'game', component: GameListComponent },
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
