import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import {GameListComponent} from "./game-list.component";

@NgModule({
  imports: [CommonModule],
declarations:[GameListComponent],
exports: [GameListComponent]
})
export class GameListModule
{
}
