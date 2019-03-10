import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AdminListComponent } from "./admin-list/admin-list.component";
import { AdminCreateComponent } from "./admin-create/admin-create.component";
import { AdminService } from "../shared/services/admin.service";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";
import { SharedModule } from "../shared/shared.module";
import { BsModalService } from 'ngx-bootstrap';
import { GameAssignComponent } from './game-assign/game-assign.component';

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, ConfirmDialogModule, SharedModule],
  declarations: [AdminListComponent, AdminCreateComponent],
  exports: [AdminListComponent, AdminCreateComponent],
  bootstrap: [AdminListComponent, AdminCreateComponent],
  providers: [AdminService, ConfirmationService, BsModalService]

})
export class AdminModule {
}
