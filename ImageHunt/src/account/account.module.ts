import { CommonModule } from "@angular/common";
import { RegistrationFormComponent } from "./registration-form/registration-form.component";
import { LoginFormComponent } from "./login-form/login-form.component";
import { UserService } from "../shared/services/user.service";
import { UserRoleComponent } from "./user-role/user-role.component";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";
import { ListboxModule } from "primeng/listbox";
import { AdminModule } from "../admin/admin.module";
import { GameAssignComponent } from "./game-assign/game-assign.component";
import { SharedModule } from "../shared/shared.module";

@NgModule({
  imports: [
    CommonModule, FormsModule, ConfirmDialogModule, ListboxModule, RouterModule, AdminModule, SharedModule,
  ],

  declarations: [RegistrationFormComponent, LoginFormComponent, UserRoleComponent, GameAssignComponent],
  exports: [RegistrationFormComponent, LoginFormComponent, UserRoleComponent, GameAssignComponent],
  bootstrap: [RegistrationFormComponent, LoginFormComponent, UserRoleComponent, GameAssignComponent],
  providers: [UserService, ConfirmationService, BsModalService]
})
export class AccountModule {
}
