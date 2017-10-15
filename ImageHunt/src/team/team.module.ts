import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {TeamDetailComponent} from "./team-detail/team.detail.component";
import {TeamService} from "../shared/services/team.service";
import {TeamListComponent} from "./team-list/team-list.component";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule],
  declarations: [TeamDetailComponent, TeamListComponent],
  exports: [TeamDetailComponent, TeamListComponent],
  providers: [TeamService]})
export class TeamModule
{
}
