import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {AgmCoreModule} from "@agm/core";
import {FormsModule} from "@angular/forms";
import {MapDetailComponent} from "./map-detail/map-detail.component";
import {environment} from "../environments/environment";
import {MapThumbnailComponent} from "./map-thumbnail/map.thumbnail.component";

@NgModule({
  imports: [CommonModule,
    AgmCoreModule.forRoot({ apiKey: environment.GOOGLE_MAP_API_KEY }),
    FormsModule],
  declarations: [MapDetailComponent, MapThumbnailComponent],
  exports: [MapDetailComponent, MapThumbnailComponent]})
export class MapModule
{
}
