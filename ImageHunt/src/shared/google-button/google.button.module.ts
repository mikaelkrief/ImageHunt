import { NgModule } from '@angular/core';
import {GoogleButtonComponent} from "./google.button.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

@NgModule({
  imports: [CommonModule, RouterModule],
  declarations: [GoogleButtonComponent],
  bootstrap: [GoogleButtonComponent],
  exports: [GoogleButtonComponent]
})
export class GoogleButtonModule
{
}
