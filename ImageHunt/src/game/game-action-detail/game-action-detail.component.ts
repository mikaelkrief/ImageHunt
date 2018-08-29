import { Component, OnInit } from '@angular/core';
import {GameService} from "../../shared/services/game.service";
import {ActivatedRoute} from "@angular/router";
import {GameAction} from "../../shared/gameAction";

@Component({
    selector: 'game-action-detail',
    templateUrl: './game-action-detail.component.html',
    styleUrls: ['./game-action-detail.component.scss']
})
/** game-action-detail component*/
export class GameActionDetailComponent implements OnInit {
  ngOnInit(): void {
    this.actionId = this.route.snapshot.params["gameActionId"];
    this.gameService.getGameAction(this.actionId)
      .subscribe((next: GameAction) => {
        this.gameAction = next;
        this.gameAction.picture.imageData = 'data:image/png;base64,' + this.gameAction.picture.image;
        this.setMap();
      });
    
  }
  /** game-action-detail ctor */
  constructor(private gameService: GameService, private route: ActivatedRoute) {
    this.gameAction = new GameAction();
    this.gameAction.latitude = 0;
    this.gameAction.longitude = 0;
    this.setMapCenter();
  }
  setMapCenter() {
    this.mapCenter = { lat: this.gameAction.latitude, lng: this.gameAction.longitude };
    this.options = {
      center: { lat: this.mapCenter.lat, lng: this.mapCenter.lng },
      zoom: 12
    };
  }

  options: any;

  overlays: any[];

  actionId: number;
  gameAction: GameAction;
  mapOptions: any;
  mapCenter: { lat: number;lng: number };

  setMap() {

  }
  validate(gameActionId: number) {
    this.gameService.validateGameAction(gameActionId).subscribe();
  }
}
