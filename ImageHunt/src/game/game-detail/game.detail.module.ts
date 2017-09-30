import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { GameDetailComponent } from "./game.detail.component";
import { GameService } from "../../shared/services/game.service";
import { FormsModule } from "@angular/forms";
import { TeamService } from "../../shared/services/team.service";
import { MapDetailModule } from "../../map/map-detail/map-detail.module";
import { BsDropdownModule, TabsModule, AlertModule } from "ngx-bootstrap";
import { NodeCreateModule } from "../node-create/node.create.module";
import {AlertService} from "../../shared/services/alert.service";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, MapDetailModule, BsDropdownModule, TabsModule, NodeCreateModule, AlertModule],
  declarations: [GameDetailComponent],
  exports: [GameDetailComponent],
  bootstrap: [GameDetailComponent],
  providers: [GameService, TeamService, AlertService]
})
export class GameDetailModule {
}
