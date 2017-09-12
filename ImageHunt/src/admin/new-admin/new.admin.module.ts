import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {NewAdminComponent} from "./new.admin.component";
import {AdminService} from "../../shared/services/admin.service";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule],
  declarations: [NewAdminComponent],
  exports: [NewAdminComponent],
  bootstrap: [NewAdminComponent],
  providers:[AdminService]
})
export class NewAdminModule
{
}
