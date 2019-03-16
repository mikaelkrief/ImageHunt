import { CommonModule } from "@angular/common";
import { ScoreListComponent } from "./score-list/score-list.component";
import { GameService } from "../shared/services/game.service";
import { TeamService } from "../shared/services/team.service";
import { AlertService } from "../shared/services/alert.service";
import { ConfirmationService } from "primeng/api";
import { SharedModule } from "../shared/shared.module";
import { MomentModule } from "angular2-moment";

@NgModule({
  imports: [CommonModule, FormsModule, SharedModule, MomentModule],
  declarations: [ScoreListComponent],
  exports: [ScoreListComponent],
  providers: [GameService, TeamService, AlertService, ConfirmationService]
})
export class ScoreModule {
}
