import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {environment} from "../environments/environment";
import {MapThumbnailComponent} from "./map-thumbnail/map.thumbnail.component";
import { GMapModule } from 'primeng/gmap';
import {MapDetail2Component} from "./map-detail2/map-detail2.component";
import { ContextMenuModule } from 'primeng/contextmenu';
import { MenuItem } from 'primeng/api';
import {} from "@types/googlemaps";
import { MapDetail3Component } from './map-detail3/map-detail3.component';
import { MapThumbnail2Component } from './map-thumbnail2/map-thumbnail2.component';

@NgModule({
  imports: [CommonModule, FormsModule, GMapModule, ContextMenuModule],
  declarations: [MapDetail2Component, MapThumbnailComponent, MapThumbnail2Component, MapDetail3Component],
  exports: [MapDetail2Component, MapThumbnailComponent, MapThumbnail2Component, MapDetail3Component]})
export class MapModule
{
}
