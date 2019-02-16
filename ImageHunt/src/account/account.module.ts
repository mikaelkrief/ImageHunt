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

@NgModule({
  imports: [
    CommonModule, FormsModule, ConfirmDialogModule, 
  ],

  declarations: [RegistrationFormComponent, LoginFormComponent, UserRoleComponent],
  exports: [RegistrationFormComponent, LoginFormComponent, UserRoleComponent],
  bootstrap: [RegistrationFormComponent, LoginFormComponent, UserRoleComponent],
  providers: [UserService, ConfirmationService, BsModalService]
})
export class AccountModule { }
