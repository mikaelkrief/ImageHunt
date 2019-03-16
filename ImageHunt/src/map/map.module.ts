import { CommonModule } from "@angular/common";
import { MapThumbnailComponent } from "./map-thumbnail/map.thumbnail.component";
import { GMapModule } from "primeng/gmap";
import { ContextMenuModule } from "primeng/contextmenu";
import { MapDetail3Component } from "./map-detail3/map-detail3.component";
import { MapThumbnail2Component } from "./map-thumbnail2/map-thumbnail2.component";

@NgModule({
  imports: [CommonModule, FormsModule, GMapModule, ContextMenuModule],
  declarations: [MapThumbnailComponent, MapThumbnail2Component, MapDetail3Component],
  exports: [MapThumbnailComponent, MapThumbnail2Component, MapDetail3Component]
})
export class MapModule {
}
