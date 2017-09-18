import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import {GameDetailComponent} from "./game.detail.component";
import {GameService} from "../../shared/services/game.service";
import { FormsModule } from "@angular/forms";
import { TeamService } from "../../shared/services/team.service";
import {MapDetailModule} from "../../map/map-detail/map-detail.module";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, MapDetailModule],
  declarations: [GameDetailComponent],
  exports: [GameDetailComponent],
  bootstrap: [GameDetailComponent],
  providers: [GameService, TeamService]
})
export class GameDetailModule
{
}
