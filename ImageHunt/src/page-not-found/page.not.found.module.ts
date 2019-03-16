import { NgModule } from '@angular/core';
import {PageNotFoundComponent} from "./page.not.found.component";
import { CommonModule } from "@angular/common";

@NgModule({
  imports: [CommonModule],
  declarations: [PageNotFoundComponent],
exports:[PageNotFoundComponent]
})
export class PageNotFoundModule
{
}
