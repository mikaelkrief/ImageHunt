import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import {TeamComponent} from "./team.component";
import {TeamService} from "./team.service";

@
NgModule({
  imports: [CommonModule],
  declarations: [TeamComponent],
  exports: [TeamComponent],
providers: [TeamService]
})
export class TeamModule
{

}
