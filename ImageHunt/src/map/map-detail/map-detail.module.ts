import { NgModule } from '@angular/core';
import { AgmCoreModule } from '@agm/core';
import { MapDetailComponent} from "./map-detail.component";
import { CommonModule } from "@angular/common";
import {environment} from "../../environments/environment";
import { FormsModule } from "@angular/forms";

@NgModule({
  imports: [CommonModule,
    AgmCoreModule.forRoot({ apiKey: environment.GOOGLE_MAP_API_KEY }),
    FormsModule],
  declarations: [MapDetailComponent],
  exports: [MapDetailComponent]
})
export class MapDetailModule
{
}
