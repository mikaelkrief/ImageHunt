import { Component, OnInit, Input } from '@angular/core';
import {GeoPoint} from "../GeoPoint";

@Component({
    selector: 'geocoordinate',
    templateUrl: './geocoordinate.component.html',
    styleUrls: ['./geocoordinate.component.scss']
})
/** geocoordinate component*/
export class GeocoordinateComponent implements OnInit {

  @Input() public Point: GeoPoint;
  @Input() public DisplayFormat: string;

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
