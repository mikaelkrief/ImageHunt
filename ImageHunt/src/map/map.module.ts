import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {environment} from "../environments/environment";
import {MapThumbnailComponent} from "./map-thumbnail/map.thumbnail.component";
import { GMapModule } from 'primeng/gmap';
import {MapDetail2Component} from "./map-detail2/map-detail2.component";
import { ContextMenuModule } from 'primeng/contextmenu';
import { MenuItem } from 'primeng/api';

@NgModule({
  imports: [CommonModule, FormsModule, GMapModule, ContextMenuModule],
  declarations: [MapDetail2Component, MapThumbnailComponent],
  exports: [MapDetail2Component, MapThumbnailComponent]})
export class MapModule
{
}
