import { NgModule } from '@angular/core';
import {MapThumbnailComponent} from "./map.thumbnail.component";
import {environment} from "../../environments/environment";
import { AgmCoreModule } from "@agm/core";
import { CommonModule } from "@angular/common";

@NgModule({
  imports: [CommonModule, AgmCoreModule.forRoot({ apiKey: environment.GOOGLE_MAP_API_KEY })],
  declarations: [MapThumbnailComponent],
  exports: [MapThumbnailComponent]
})
export class MapThumbnailModule
{
}
