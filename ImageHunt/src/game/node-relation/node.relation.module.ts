import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { BsDropdownModule, TabsModule } from "ngx-bootstrap";
import {NodeRelationComponent} from "./node.relation.component";

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, BsDropdownModule, TabsModule],
  declarations: [NodeRelationComponent],
  exports: [NodeRelationComponent],
  entryComponents: [NodeRelationComponent]
})
export class NodeRelationModule
{
}
