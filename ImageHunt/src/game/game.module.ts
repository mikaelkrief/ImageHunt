import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {RouterModule} from "@angular/router";
import {CalendarModule} from "primeng/calendar";
import {GameCreateComponent} from "./game-create/game.create.component";
import {GameService} from "../shared/services/game.service";
import {MapModule} from "../map/map.module";
import { BsDropdownModule, TabsModule, AlertModule, BsDatepickerModule  } from "ngx-bootstrap";
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
import { GameActionListComponent } from "./game-action-list/game-action-list.component";
import {GameActionDetailComponent} from "./game-action-detail/game-action-detail.component";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";
import { RadioButtonModule } from 'primeng/radiobutton';
import { ToggleButtonModule } from "primeng/togglebutton";
import { DropdownModule } from "primeng/dropdown";
import {SharedModule} from "../shared/shared.module";
import { TeamModule } from "../team/team.module";
import { LightboxModule } from 'primeng/lightbox';
import { ImageNodeEditComponent } from './image-node-edit/image-node-edit.component';
import { LiveService } from '../shared/services/live.service';
import { PasscodeListComponent } from './passcode-list/passcode-list.component';
import { PasscodeCreateComponent } from './passcode-create/passcode-create.component';
import { PasscodePrintComponent } from './passcode-print/passcode-print.component';
import { QRCodeModule } from 'angular2-qrcode';

@NgModule({
  imports: [CommonModule, FormsModule, RouterModule, CalendarModule, CommonModule, RouterModule, FormsModule,
    MapModule, BsDropdownModule, TabsModule, AlertModule, BrowserModule, ConfirmDialogModule,
    ToggleButtonModule, DropdownModule, ContextMenuModule, TableModule, GMapModule, SharedModule,
    RadioButtonModule, TeamModule, LightboxModule, QRCodeModule, BsDatepickerModule],
  declarations: [GameCreateComponent, GameDetailComponent, GameListComponent, NodeCreateComponent,
    NodeListComponent, NodeRelationComponent, QuestionNodeComponent, GameActionListComponent,
    GameActionDetailComponent, ImageNodeEditComponent, PasscodeListComponent, PasscodeCreateComponent, PasscodePrintComponent],
  exports: [GameCreateComponent, GameDetailComponent, GameListComponent,
    NodeCreateComponent, NodeListComponent, NodeRelationComponent, QuestionNodeComponent,
    GameActionListComponent, GameActionDetailComponent, ImageNodeEditComponent, PasscodeListComponent, PasscodeCreateComponent,
    PasscodePrintComponent],
  providers: [GameService, TeamService, AlertService, ConfirmationService, LiveService]
})
export class GameModule
{
}
