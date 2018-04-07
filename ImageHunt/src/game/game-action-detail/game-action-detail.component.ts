import { Component, OnInit } from '@angular/core';
import * as Gameservice from "../../shared/services/game.service";
import GameService = Gameservice.GameService;
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
      .subscribe(next=> this.gameAction = next.json());
  }
  /** game-action-detail ctor */
  constructor(private gameService: GameService, private route: ActivatedRoute) {

    }

  actionId: number;
  gameAction: GameAction;
}
