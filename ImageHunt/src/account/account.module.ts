import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrationFormComponent } from './registration-form/registration-form.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { FormsModule } from '@angular/forms';
import { UserService } from "../shared/services/user.service";
import { UserRoleComponent } from './user-role/user-role.component';
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";
import { BsModalService } from 'ngx-bootstrap';
import { ListboxModule } from 'primeng/listbox';
import { RouterModule } from '@angular/router';
import { AdminModule } from "../admin/admin.module";
import { GameAssignComponent } from './game-assign/game-assign.component';
import { SharedModule } from "../shared/shared.module";
import { EditUserComponent } from './edit-user/edit-user.component';

@NgModule({
  imports: [
    CommonModule, FormsModule, ConfirmDialogModule, ListboxModule, RouterModule, AdminModule, SharedModule,
  ],

  declarations: [RegistrationFormComponent, LoginFormComponent, UserRoleComponent, GameAssignComponent, EditUserComponent],
  exports: [RegistrationFormComponent, LoginFormComponent, UserRoleComponent, GameAssignComponent, EditUserComponent],
  bootstrap: [RegistrationFormComponent, LoginFormComponent, UserRoleComponent, GameAssignComponent, EditUserComponent],
  providers: [UserService, ConfirmationService, BsModalService]
})
export class AccountModule { }
