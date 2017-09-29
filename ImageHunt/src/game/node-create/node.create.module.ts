import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import {NodeCreateComponent} from "./node.create.component";
import { BsDropdownModule, TabsModule } from "ngx-bootstrap";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, BsDropdownModule, TabsModule],
  declarations: [NodeCreateComponent],
  exports: [NodeCreateComponent],
  entryComponents: [NodeCreateComponent]
})
export class NodeCreateModule
{
}
