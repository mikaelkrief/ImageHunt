import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { PlayerScoreboardComponent } from "./player-scoreboard/player-scoreboard.component";
import { AlertService } from "../shared/services/alert.service";
import { ConfirmationService } from "primeng/api";
import { NgModule } from "@angular/core";

@NgModule({
    imports: [CommonModule, FormsModule, RouterModule],
    declarations: [PlayerScoreboardComponent],
    exports: [PlayerScoreboardComponent],
    providers: [AlertService, ConfirmationService]
  })
  export class PlayerModule
  {
  }