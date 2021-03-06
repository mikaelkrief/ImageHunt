import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {environment} from "../environments/environment";
import {MapThumbnailComponent} from "./map-thumbnail/map.thumbnail.component";
import { GMapModule } from 'primeng/gmap';
import { ContextMenuModule } from 'primeng/contextmenu';
import { MenuItem, ConfirmationService } from 'primeng/api';
import { MapDetail3Component } from './map-detail3/map-detail3.component';
import { MapThumbnail2Component } from './map-thumbnail2/map-thumbnail2.component';
import { ConfirmDialogModule } from 'primeng/primeng';

@NgModule({
  imports: [CommonModule, FormsModule, GMapModule, ContextMenuModule, ConfirmDialogModule],
  declarations: [MapThumbnailComponent, MapThumbnail2Component, MapDetail3Component],
  exports: [MapThumbnailComponent, MapThumbnail2Component, MapDetail3Component],
  providers: [ConfirmationService]
})
export class MapModule
{
}
