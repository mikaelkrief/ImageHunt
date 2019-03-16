import * as L from "leaflet";

@Component({
  selector: "map-thumbnail2",
  templateUrl: "./map-thumbnail2.component.html",
  styleUrls: ["./map-thumbnail2.component.scss"]
})
/** map-thumbnail2 component*/
export class MapThumbnail2Component implements OnInit {
  @Input()
  CenterLat: number;
  @Input()
  CenterLng: number;
  @Input()
  MapName: string;
  MapThumbnailId: string;

  ngOnInit(): void {


  }

  ngAfterContentInit() {
    const mapThumbnail = L.map(this.MapThumbnailId).setView([this.CenterLat, this.CenterLng], 12);

    L.tileLayer("http://{s}.tile.osm.org/{z}/{x}/{y}.png",
      {
        attribution: "ImageHunt"
      }).addTo(mapThumbnail);

    mapThumbnail.dragging.disable();
    mapThumbnail.touchZoom.disable();
    mapThumbnail.zoomControl.remove();
    mapThumbnail.doubleClickZoom.disable();
    mapThumbnail.scrollWheelZoom.disable();
  }

  /** map-thumbnail2 ctor */
  constructor() {
    this.MapThumbnailId = Math.floor((1 + Math.random()) * 0x10000)
      .toString(16)
      .substring(1);
  }
}
