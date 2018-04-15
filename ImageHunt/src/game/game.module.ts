import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {RouterModule} from "@angular/router";
import {CalendarModule} from "primeng/calendar";
import {GameCreateComponent} from "./game-create/game.create.component";
import {GameService} from "../shared/services/game.service";
import {MapModule} from "../map/map.module";
import {BsDropdownModule, TabsModule, AlertModule } from "ngx-bootstrap";
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
import {PipeModule} from "../shared/pipes/pipe.module";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";
import { RadioButtonModule } from 'primeng/radiobutton';
import {GeocoordinateModule} from "../shared/geocoordinate/geocoordinate.module";
import { ToggleButtonModule } from "primeng/togglebutton";

@NgModule({
  imports: [CommonModule, FormsModule, RouterModule, CalendarModule, CommonModule, RouterModule, FormsModule,
    MapModule, BsDropdownModule, TabsModule, AlertModule, BrowserModule, ConfirmDialogModule, ToggleButtonModule,
    ContextMenuModule, TableModule, GMapModule, PipeModule.forRoot(), RadioButtonModule, GeocoordinateModule],
  declarations: [GameCreateComponent, GameDetailComponent, GameListComponent, NodeCreateComponent, NodeListComponent, NodeRelationComponent, QuestionNodeComponent, GameActionListComponent, GameActionDetailComponent],
  exports: [GameCreateComponent, GameDetailComponent, GameListComponent, NodeCreateComponent, NodeListComponent, NodeRelationComponent, QuestionNodeComponent, GameActionListComponent, GameActionDetailComponent],
  providers: [GameService, TeamService, AlertService, ConfirmationService]
})
export class GameModule
{
}
