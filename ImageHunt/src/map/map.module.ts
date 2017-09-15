import { NgModule } from '@angular/core';
import { AgmCoreModule } from '@agm/core';
import {MapComponent} from "./map.component";
import { CommonModule } from "@angular/common";
import {environment} from "../environments/environment";

@NgModule({
  imports: [CommonModule, AgmCoreModule.forRoot({ apiKey: environment.GOOGLE_MAP_API_KEY })],
  declarations: [MapComponent],
  exports: [MapComponent]
})
export class MapModule
{
}
