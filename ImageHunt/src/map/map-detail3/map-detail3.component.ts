import { Component, OnInit, Input } from '@angular/core';
import * as L from 'leaflet';
import { GameService } from '../../shared/services/game.service';

@Component({
    selector: 'map-detail3',
    templateUrl: './map-detail3.component.html',
    styleUrls: ['./map-detail3.component.scss']
})
/** map-detail3 component*/
export class MapDetail3Component implements OnInit {
  @Input() gameId: number;
    ngOnInit(): void {
    const mapThumbnail = L.map("MapDetail")
      .setView([0, 0], 12);

    L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
      attribution: 'ImageHunt'
    }).addTo(mapThumbnail);

  }
    /** map-detail3 ctor */
  constructor(private _gameService: GameService) {

    }
}
