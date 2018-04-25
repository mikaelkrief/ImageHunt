import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'map-thumbnail',
    templateUrl: './map.thumbnail.component.html',
    styleUrls: ['./map.thumbnail.component.scss']
})
/** map-thumbnail component*/
export class MapThumbnailComponent implements OnInit {
  options: any;
  @Input() public CenterLat: number;
  @Input() public CenterLng: number;

    /** map-thumbnail ctor */
    constructor() { }

    /** Called by Angular after map-thumbnail component initialized */
    ngOnInit(): void {
      this.options = {
        center: { lat: this.CenterLat, lng: this.CenterLng },
        zoom: 12
      };

    }


}
