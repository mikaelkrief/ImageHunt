import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
//import { AppComponent } from './app.component';
import { TeamComponent } from '../team/team.component';

import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import {TeamService} from "../team/team.service";

@NgModule({
  declarations: [
    //AppComponent,
    TeamComponent
    ],
  bootstrap: [
      //AppComponent,
    TeamComponent
  ],
  imports: [
      BrowserModule,
      FormsModule,
      HttpModule
  ],
  providers: [TeamService]
})
export class AppModule { }
