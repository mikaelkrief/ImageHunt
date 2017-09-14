import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import {GameListComponent} from "./game-list.component";
import {GameCreateModule} from "../game-create/game.create.module";

@NgModule({
  imports: [CommonModule, GameCreateModule],
declarations:[GameListComponent],
exports: [GameListComponent]
})
export class GameListModule
{
}
