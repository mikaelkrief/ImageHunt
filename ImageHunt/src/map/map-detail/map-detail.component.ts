import { Component, OnInit, Input } from '@angular/core';
import {Node} from "../../shared/node";

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
    }
  getIconForNodeType(nodeType: string): string {
    switch (nodeType) {
    case "TimerNode":
        return "assets/timerNode.png";
    case "PictureNode":
      return "assets/pictureNode.png";
    case "FirstNode":
      return "assets/startNode.png";
    case "LastNode":
        return "assets/endNode.png";
    case "QuestionNode":
        return "assets/questionNode.png";
    case "ObjectNode":
        return "assets/objectNode.png";
    default:
      return null;
    }
  }
}
