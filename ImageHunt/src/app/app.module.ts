import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';

import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import {HomeModule} from "../home/home.module";
import {GameModule} from "../game/game.module";
import {PageNotFoundModule} from "../page-not-found/page.not.found.module";
import {TeamModule} from "../team/team.module";
import {TeamComponent} from "../team/team.component";
import {HomeComponent} from "../home/home.component";
import {GameComponent} from "../game/game.component";
import {PageNotFoundComponent} from "../page-not-found/page.not.found.component";
import {NavmenuModule} from "../navmenu/navmenu.module";

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
    TeamModule,
    NavmenuModule,
      PageNotFoundModule,
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent },
      { path: 'game', component: GameComponent },
      {path: 'team', component: TeamComponent},
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      {path: '**', component: PageNotFoundComponent}])
                         ],
  providers: []
})
export class AppModule { }
