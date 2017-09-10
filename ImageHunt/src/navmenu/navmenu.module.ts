import { NgModule } from '@angular/core';
import {NavmenuComponent} from "./navmenu.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

@NgModule({
  imports: [CommonModule, RouterModule],
  declarations: [NavmenuComponent],
  exports: [NavmenuComponent],
  bootstrap: [NavmenuComponent]
})
export class NavmenuModule
{
}
