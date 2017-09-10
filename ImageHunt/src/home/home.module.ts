import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import {HomeComponent} from "./home.component";

@NgModule({
  imports: [CommonModule],
  declarations: [HomeComponent],
  exports: [HomeComponent],
  bootstrap: [HomeComponent]

})
export class HomeModule
{
}
