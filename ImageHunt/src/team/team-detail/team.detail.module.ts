import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import {TeamDetailComponent} from "./team.detail.component";
import {TeamService} from "../../shared/services/team.service";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule],
  declarations: [TeamDetailComponent],
  exports: [TeamDetailComponent],
  providers:[TeamService]
})
export class TeamDetailModule
{

}
