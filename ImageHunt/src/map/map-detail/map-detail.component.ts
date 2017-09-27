import { Component, OnInit, Input, Output, EventEmitter, TemplateRef } from '@angular/core';
import { Node } from "../../shared/node";
import {GameService} from "../../shared/services/game.service";
import { BsModalService, BsModalRef } from "ngx-bootstrap";
import { Subscription } from "rxjs/Subscription";

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
  public modalRef: BsModalRef;
  currentLatitude: number;
  currentLongitude: number;
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
  getGameData(gameId: number) {
    if (gameId != null) {
      this._gameService.getGameById(gameId)
        .subscribe(res => {
          this.CenterLat = res.mapCenterLat;
          this.CenterLng = res.mapCenterLng;
          this.Nodes = res.nodes;
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
    var coordinates = event.coords;
    this.currentLatitude = coordinates.lat;
    this.currentLongitude = coordinates.lng;
    this.subscriptions.push(this._modalService.onHide.subscribe((reason: string) => {
      this.createNode();
    }));
    this.subscriptions.push(this._modalService.onHidden.subscribe((reason: string) => this.unsubscribe()));
    this.modalRef = this._modalService.show(templateName, { ignoreBackdropClick:true});
  }
  createNode() {
    var node = {
      nodeType: 'QuestionNode',
      name: 'From TypeScript',
      latitude: this.currentLatitude,
      longitude: this.currentLongitude
    };
    this._gameService.addNode(this.gameId, node)
      .subscribe(null,
        null,
        () => this.getGameData(this.gameId));
    
  }
}
