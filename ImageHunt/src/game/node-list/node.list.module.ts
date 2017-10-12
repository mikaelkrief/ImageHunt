import { NgModule} from '@angular/core';
import { CommonModule } from "@angular/common";
import {NodeListComponent} from "./node.list.component";

@NgModule({
  imports: [CommonModule],
  declarations:[NodeListComponent],
  exports: [NodeListComponent],
  entryComponents: [NodeListComponent]
})
export class NodeListModule
{
}
