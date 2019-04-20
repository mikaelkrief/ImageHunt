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
import { BotCommandComponent } from './bot-command/bot-command.component';
import { MarkdownModule } from 'ngx-markdown';

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, ConfirmDialogModule, SharedModule, MarkdownModule ],
  declarations: [AdminListComponent, AdminCreateComponent, BotCommandComponent],
  exports: [AdminListComponent, AdminCreateComponent, BotCommandComponent],
  bootstrap: [AdminListComponent, AdminCreateComponent, BotCommandComponent],
  providers: [AdminService, ConfirmationService, BsModalService]

})
export class AdminModule {
}
