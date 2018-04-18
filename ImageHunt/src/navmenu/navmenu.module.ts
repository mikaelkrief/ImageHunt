import { NgModule } from '@angular/core';
import {NavmenuComponent} from "./navmenu.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { CollapseModule } from 'ngx-bootstrap';
import {SharedModule} from "../shared/shared.module";

@NgModule({
  imports: [CommonModule, RouterModule, SharedModule, CollapseModule],
  declarations: [NavmenuComponent],
  exports: [NavmenuComponent],
  bootstrap: [NavmenuComponent]
})
export class NavmenuModule
{
}
