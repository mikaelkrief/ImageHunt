import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {GameCreateComponent} from "./game.create.component";
import {GameService} from "../../shared/services/game.service";
  
@NgModule({
  imports: [CommonModule, RouterModule],
  declarations: [GameCreateComponent],
  exports: [GameCreateComponent],
  bootstrap: [GameCreateComponent],
  providers: [GameService]

})
export class GameCreateModule
{
}
