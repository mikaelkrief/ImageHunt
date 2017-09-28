import { Component, OnInit, Input, Output, EventEmitter, TemplateRef } from '@angular/core';
import { Node } from "../../shared/node";
import {GameService} from "../../shared/services/game.service";
import { BsModalService, BsModalRef } from "ngx-bootstrap";
import { Subscription } from "rxjs/Subscription";
import { NgForm } from "@angular/forms";
import {NodeRelation} from "../../shared/NodeRelation";

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
  @Input() nodes: Node[];
  @Input() nodesRelation: NodeRelation[];
  @Input() nodeMode: string;
  @Input() filterNode: string[];
  @Output() mapClicked = new EventEmitter();
  @Output() nodeClicked = new EventEmitter<Node>();
  /** map ctor */
  constructor(private _gameService: GameService, private _modalService: BsModalService) { }

  /** Called by Angular after map component initialized */
  ngOnInit(): void {
    this.getGameData(this.gameId);
    if (this.CenterLat == null) {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(position => {
          this.CenterLat = position.coords.latitude;
          this.CenterLng = position.coords.longitude;
        },
          err => { console.error(err); this.CenterLat = 51.4872846;
            this.CenterLng = -0.1197003;
          }
        );
      }
    }
  }
  public getGameData(gameId: number) {
    if (gameId != null) {
      this._gameService.getGameById(gameId)
        .subscribe(res => {
          this.CenterLat = res.mapCenterLat;
          this.CenterLng = res.mapCenterLng;
          this.nodes = res.nodes;
        });
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
  public subscriptions: Subscription[] = [];
  public unsubscribe() {
    this.subscriptions.forEach((subscription: Subscription) => {
      subscription.unsubscribe();
    });
    this.subscriptions = [];
  }
  mapClick(event, templateName:TemplateRef<any>) {
    this.mapClicked.emit(event);
  }
  markerClicked(node: Node) {
    this.nodeClicked.emit(node);
  }
}
