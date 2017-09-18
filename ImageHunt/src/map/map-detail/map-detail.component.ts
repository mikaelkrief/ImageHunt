import { Component, OnInit, Input, ViewChild } from '@angular/core';
import {Node} from "../../shared/node";
import { AgmMap, AgmMarker, MarkerManager } from "@agm/core";

@Component({
    selector: 'map-detail',
    templateUrl: './map-detail.component.html',
    styleUrls: ['./map-detail.component.scss']
})
/** map component*/
export class MapDetailComponent implements OnInit
{
  @Input() CenterLat: number;
  @Input() CenterLng: number;
  @Input() Nodes: Node[];
  @ViewChild('map') map: AgmMap;
    /** map ctor */
    constructor() { }

    /** Called by Angular after map component initialized */
    ngOnInit(): void {
      if (this.CenterLat == null) {
        if (navigator.geolocation) {
          navigator.geolocation.getCurrentPosition(position => {
            this.CenterLat = position.coords.latitude;
            this.CenterLng = position.coords.longitude;
          },
            err => { this.CenterLat = 51.4872846; this.CenterLng = -0.1197003}
          );
        }
      }
      if (this.Nodes) {
        this.insertNodes(this.Nodes);
      }
    }

    insertNodes(nodes: Node[]) {
    //for (var node of nodes) {
    //};
    
  }
}
