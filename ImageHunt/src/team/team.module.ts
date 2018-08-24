import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import {TeamDetailComponent} from "./team-detail/team.detail.component";
import {TeamService} from "../shared/services/team.service";
import {TeamListComponent} from "./team-list/team-list.component";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import {ConfirmationService} from "primeng/api";
import { TeamCreateComponent } from "./team-create/team-create.component";
import { PanelModule } from "primeng/panel";
import {QRCodeModule } from "angular2-qrcode";
import { LiveService } from '../shared/services/live.service';
import { TeamFollowComponent } from './team-follow/team-follow.component';
import { ColorPickerModule } from 'primeng/colorpicker';

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ConfirmDialogModule,
    PanelModule, QRCodeModule, ReactiveFormsModule, ColorPickerModule],
  declarations: [TeamDetailComponent, TeamListComponent, TeamCreateComponent, TeamFollowComponent],
  exports: [TeamDetailComponent, TeamListComponent, TeamCreateComponent, TeamFollowComponent],
  providers: [TeamService, ConfirmationService, LiveService]})
export class TeamModule
{
}
