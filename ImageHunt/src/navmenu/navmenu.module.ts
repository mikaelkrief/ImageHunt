import { NgModule } from '@angular/core';
import {NavmenuComponent} from "./navmenu.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { GoogleButtonModule } from "../shared/google-button/google.button.module";
import { CollapseModule } from 'ngx-bootstrap';

@NgModule({
  imports: [CommonModule, RouterModule, GoogleButtonModule, CollapseModule],
  declarations: [NavmenuComponent],
  exports: [NavmenuComponent],
  bootstrap: [NavmenuComponent]
})
export class NavmenuModule
{
}
