import { CommonModule } from "@angular/common";
import { AdminListComponent } from "./admin-list/admin-list.component";
import { AdminCreateComponent } from "./admin-create/admin-create.component";
import { AdminService } from "../shared/services/admin.service";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";
import { SharedModule } from "../shared/shared.module";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, ConfirmDialogModule, SharedModule],
  declarations: [AdminListComponent, AdminCreateComponent],
  exports: [AdminListComponent, AdminCreateComponent],
  bootstrap: [AdminListComponent, AdminCreateComponent],
  providers: [AdminService, ConfirmationService, BsModalService]

})
export class AdminModule {
}
