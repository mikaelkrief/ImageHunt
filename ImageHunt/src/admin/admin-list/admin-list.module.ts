import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import {AdminListComponent} from "./admin-list.component";
import {AdminService} from "../../shared/services/admin.service";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule],
  declarations: [AdminListComponent],
  exports: [AdminListComponent],
  bootstrap: [AdminListComponent],
  providers: [AdminService]

})
export class AdminListModule
{
}
