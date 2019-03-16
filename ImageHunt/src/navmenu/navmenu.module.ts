import { NgModule } from '@angular/core';
import {NavmenuComponent} from "./navmenu.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { CollapseModule, BsModalService } from 'ngx-bootstrap';
import {SharedModule} from "../shared/shared.module";
import { AccountModule } from '../account/account.module';

@NgModule({
  imports: [CommonModule, RouterModule, SharedModule, CollapseModule, AccountModule],
  declarations: [NavmenuComponent],
  exports: [NavmenuComponent],
  bootstrap: [NavmenuComponent],
  providers: [BsModalService]

})
export class NavmenuModule
{
}
