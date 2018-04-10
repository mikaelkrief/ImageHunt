import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import {AdminListComponent} from "./admin-list.component";
import {AdminService} from "../../shared/services/admin.service";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ConfirmDialogModule],
  declarations: [AdminListComponent],
  exports: [AdminListComponent],
  bootstrap: [AdminListComponent],
  providers: [AdminService, ConfirmationService]

})
export class AdminListModule
{
}
