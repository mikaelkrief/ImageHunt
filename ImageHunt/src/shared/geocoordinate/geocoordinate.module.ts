import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import {GeocoordinateComponent} from "./geocoordinate.component";

@NgModule({
  imports: [CommonModule],
  declarations: [GeocoordinateComponent],
  bootstrap: [GeocoordinateComponent],
  exports: [GeocoordinateComponent]
})
export class GeocoordinateModule {
}
