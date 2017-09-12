import { NgModule } from '@angular/core';
import {NavmenuComponent} from "./navmenu.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import {GoogleButtonModule} from "../shared/google-button/google.button.module";

@NgModule({
  imports: [CommonModule, RouterModule, GoogleButtonModule],
  declarations: [NavmenuComponent],
  exports: [NavmenuComponent],
  bootstrap: [NavmenuComponent]
})
export class NavmenuModule
{
}
