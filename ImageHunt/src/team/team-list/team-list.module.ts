import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import {TeamListComponent} from "./team-list.component";
import {TeamService} from "../../shared/services/team.service";

@
NgModule({
  imports: [CommonModule],
  declarations: [TeamListComponent],
  exports: [TeamListComponent],
providers: [TeamService]
})
export class TeamListModule
{

}
