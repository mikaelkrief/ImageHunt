import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {TeamDetailComponent} from "./team-detail/team.detail.component";
import {TeamService} from "../shared/services/team.service";
import {TeamListComponent} from "./team-list/team-list.component";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import {ConfirmationService} from "primeng/api";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ConfirmDialogModule],
  declarations: [TeamDetailComponent, TeamListComponent],
  exports: [TeamDetailComponent, TeamListComponent],
  providers: [TeamService, ConfirmationService]})
export class TeamModule
{
}
