import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {RouterModule} from "@angular/router";
import {CalendarModule} from "primeng/calendar";
import {GameCreateComponent} from "./game-create/game.create.component";
import {GameService} from "../shared/services/game.service";
import {MapModule} from "../map/map.module";
import { BsDropdownModule, TabsModule, AlertModule, BsDatepickerModule, TooltipModule, AccordionModule, ModalModule  } from "ngx-bootstrap";
import {GameDetailComponent} from "./game-detail/game.detail.component";
import {TeamService} from "../shared/services/team.service";
import {AlertService} from "../shared/services/alert.service";
import {GameListComponent} from "./game-list/game-list.component";
import {NodeCreateComponent} from "./node-create/node.create.component";
import {NodeListComponent} from "./node-list/node.list.component";
import {NodeRelationComponent} from "./node-relation/node.relation.component";
import { BrowserModule } from "@angular/platform-browser";
import {QuestionNodeComponent} from "./question-node/question.node.component";
import { ContextMenuModule } from "primeng/primeng";
import { TableModule } from "primeng/table";
import { GMapModule } from 'primeng/gmap';
import { InputSwitchModule } from 'primeng/inputswitch';

import { GameActionListComponent } from "./game-action-list/game-action-list.component";
import {GameActionDetailComponent} from "./game-action-detail/game-action-detail.component";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";
import { RadioButtonModule } from 'primeng/radiobutton';
import { ToggleButtonModule } from "primeng/togglebutton";
import { PanelModule } from 'primeng/panel';
import { DropdownModule } from "primeng/dropdown";
import {SharedModule} from "../shared/shared.module";
import { TeamModule } from "../team/team.module";
import { LightboxModule } from 'primeng/lightbox';
import { ImageNodeEditComponent } from './image-node-edit/image-node-edit.component';
import { LiveService } from '../shared/services/live.service';
import { PasscodeListComponent } from './passcode-list/passcode-list.component';
import { PasscodeCreateComponent } from './passcode-create/passcode-create.component';
import { PasscodePrintComponent } from './passcode-print/passcode-print.component';
import { ButtonsModule } from 'ngx-bootstrap';
import { ImageService } from 'services/image.service';
import { GameAvailableComponent } from './game-available/game-available.component';
import { GameValidationComponent } from './game-validation/game-validation.component';
import { PointsComponent } from './points/points.component';
import { BatchNodeComponent } from "./batch-node/batch-node.component";
import { NodeEditComponent } from "./node-edit/node-edit.component";

@
NgModule({
  imports: [
    CommonModule, FormsModule, RouterModule, CalendarModule, CommonModule, RouterModule, FormsModule,
    MapModule, BsDropdownModule, TabsModule, AlertModule, BrowserModule, ConfirmDialogModule,
    ToggleButtonModule, DropdownModule, ContextMenuModule, TableModule, GMapModule, SharedModule,
    RadioButtonModule, TeamModule, LightboxModule, ButtonsModule, TooltipModule, PanelModule,
    AccordionModule, InputSwitchModule, ModalModule.forRoot()
  ],
  declarations: [
    GameCreateComponent, GameDetailComponent, GameListComponent, NodeCreateComponent, NodeEditComponent,
    NodeListComponent, NodeRelationComponent, QuestionNodeComponent, GameActionListComponent,
    GameActionDetailComponent, ImageNodeEditComponent, PasscodeListComponent, PasscodeCreateComponent,
    PasscodePrintComponent, GameAvailableComponent, GameValidationComponent, PointsComponent,
    BatchNodeComponent
  ],
  exports: [
    GameCreateComponent, GameDetailComponent, GameListComponent,
    NodeCreateComponent, NodeEditComponent, NodeListComponent, NodeRelationComponent, QuestionNodeComponent,
    GameActionListComponent, GameActionDetailComponent, ImageNodeEditComponent, PasscodeListComponent,
    PasscodeCreateComponent,
    PasscodePrintComponent, GameAvailableComponent, GameValidationComponent, PointsComponent, BatchNodeComponent
  ],
  providers: [GameService, TeamService, AlertService, ConfirmationService, LiveService, ImageService]
})
export class GameModule
{
}
