import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import {GameDetailComponent} from "./game.detail.component";
import {GameService} from "../../shared/services/game.service";

@NgModule({
  imports: [CommonModule, RouterModule],
  declarations: [GameDetailComponent],
  exports: [GameDetailComponent],
  bootstrap: [GameDetailComponent],
  providers: [GameService]
})
export class GameDetailModule
{
}
