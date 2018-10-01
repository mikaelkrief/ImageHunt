import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import {AdminListComponent} from "./admin-list/admin-list.component";
import {AdminCreateComponent} from "./admin-create/admin-create.component";
import {AdminService} from "../shared/services/admin.service";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";
import {SharedModule} from "../shared/shared.module";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ConfirmDialogModule, SharedModule],
  declarations: [AdminListComponent, AdminCreateComponent],
  exports: [AdminListComponent, AdminCreateComponent],
  bootstrap: [AdminListComponent, AdminCreateComponent],
  providers: [AdminService, ConfirmationService]

})
export class AdminModule
{
}
