import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import {TeamDetailComponent} from "./team.detail.component";

@NgModule({
  imports: [CommonModule, RouterModule],
  declarations: [TeamDetailComponent],
  exports: [TeamDetailComponent]})
export class TeamDetailModule
{

}
