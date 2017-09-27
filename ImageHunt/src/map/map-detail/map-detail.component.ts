import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Node } from "../../shared/node";
import {GameService} from "../../shared/services/game.service";

@Component({
  selector: 'map-detail',
  templateUrl: './map-detail.component.html',
  styleUrls: ['./map-detail.component.scss']
})
/** map component*/
export class MapDetailComponent implements OnInit {
  @Input() CenterLat: number;
  @Input() CenterLng: number;
  @Input() gameId: number;
  @Input() Nodes: Node[];
  @Input() nodeMode: string;
  @Output() mapClicked = new EventEmitter();
  /** map ctor */
  constructor(private _gameService: GameService) { }

  /** Called by Angular after map component initialized */
  ngOnInit(): void {
    if (this.CenterLat == null) {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(position => {
          this.CenterLat = position.coords.latitude;
          this.CenterLng = position.coords.longitude;
        },
          err => { this.CenterLat = 51.4872846; this.CenterLng = -0.1197003 }
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
  mapClick(event) {
    this.mapClicked.emit(event);
    var coordinates = event.coords;
    var node = {
      nodeType: 'QuestionNode',
      name: 'From TypeScript',
      latitude: coordinates.lat,
      longitude: coordinates.lng
    };
    this._gameService.addNode(this.gameId, node)
      .subscribe(null,
        null,
        () => this._gameService.getGameById(this.gameId).subscribe(res => this.Nodes = res.nodes, null, null));
  }
}
