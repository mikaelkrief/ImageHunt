import { GeoPoint } from "../GeoPoint";

@Component({
  selector: "geocoordinate",
  templateUrl: "./geocoordinate.component.html",
  styleUrls: ["./geocoordinate.component.scss"]
})
/** geocoordinate component*/
export class GeocoordinateComponent implements OnInit {

  @Input()
  Point: GeoPoint;
  @Input()
  DisplayFormat: string;

  displayLatitude: string;
  displayLongitude: string;

  /** geocoordinate ctor */
  constructor() {

  }

  ngOnInit(): void {
    switch (this.DisplayFormat) {
    case "sexagecimal":
      break;
    case "decimal":
    default:
    }
  }
}
