import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ScoreListComponent } from "./score-list/score-list.component";
import { GameService } from "../shared/services/game.service";
import { TeamService } from "../shared/services/team.service";
import { AlertService } from "../shared/services/alert.service";
import { ConfirmationService } from "primeng/api";
import { SharedModule } from "../shared/shared.module";

@NgModule({
  imports: [CommonModule, FormsModule, SharedModule],
  declarations: [ScoreListComponent],
  exports: [ScoreListComponent],
  providers: [GameService, TeamService, AlertService, ConfirmationService]
})
export class ScoreModule {
}
