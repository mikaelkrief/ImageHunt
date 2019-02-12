import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrationFormComponent } from './registration-form/registration-form.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { FormsModule } from '@angular/forms';
import { UserService } from "../shared/services/user.service";

@NgModule({
  imports: [
    CommonModule, FormsModule
  ],

  declarations: [RegistrationFormComponent, LoginFormComponent],
  exports: [RegistrationFormComponent, LoginFormComponent],
  bootstrap: [RegistrationFormComponent, LoginFormComponent],
  providers: [ UserService]
})
export class AccountModule { }
