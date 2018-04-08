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
      .subscribe(next => this.gameAction = next.json());
    this.setMapCenter();
  }
  /** game-action-detail ctor */
  constructor(private gameService: GameService, private route: ActivatedRoute) {
    this.gameAction = new GameAction();
    this.setMapCenter();
  }
  setMapCenter() {
    this.mapCenter = { lat: this.gameAction.latitude, lng: this.gameAction.longitude };

  }
  actionId: number;
  gameAction: GameAction;
  mapOptions: any;
  mapCenter: { lat: number;lng: number };
}
