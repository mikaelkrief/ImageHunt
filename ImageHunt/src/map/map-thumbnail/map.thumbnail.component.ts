@Component({
  selector: "map-thumbnail",
  templateUrl: "./map.thumbnail.component.html",
  styleUrls: ["./map.thumbnail.component.scss"]
})
/** map-thumbnail component*/
export class MapThumbnailComponent implements OnInit {
  options: any;
  @Input()
  CenterLat: number;
  @Input()
  CenterLng: number;

  /** map-thumbnail ctor */
  constructor() {}

  /** Called by Angular after map-thumbnail component initialized */
  ngOnInit(): void {
    this.options = {
      center: { lat: this.CenterLat, lng: this.CenterLng },
      zoom: 12,
      fullscreenControl: false,
      zoomControl: false,
      mapTypeControl: false,
      scaleControl: false,
      streetViewControl: false,
      rotateControl: false
    };

  }


}
