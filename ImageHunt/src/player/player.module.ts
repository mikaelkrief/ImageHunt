import { CommonModule } from "@angular/common";
import { PlayerScoreboardComponent } from "./player-scoreboard/player-scoreboard.component";
import { AlertService } from "../shared/services/alert.service";
import { ConfirmationService } from "primeng/api";

@NgModule({
  imports: [CommonModule, FormsModule, RouterModule],
  declarations: [PlayerScoreboardComponent],
  exports: [PlayerScoreboardComponent],
  providers: [AlertService, ConfirmationService]
})
export class PlayerModule {
}
