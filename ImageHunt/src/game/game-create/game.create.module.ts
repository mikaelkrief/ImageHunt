import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from '@angular/forms';
import {GameCreateComponent} from "./game.create.component";
import { GameService } from "../../shared/services/game.service";
import { CalendarModule } from 'primeng/primeng';

@NgModule({
  imports: [CommonModule, FormsModule, RouterModule, CalendarModule],
  declarations: [GameCreateComponent],
  exports: [GameCreateComponent],
  bootstrap: [GameCreateComponent],
  providers: [GameService]

})
export class GameCreateModule
{
}
