import { Component, OnInit } from '@angular/core';
import {GameService} from "../../shared/services/game.service";
import { ActivatedRoute } from "@angular/router";
import { GameAction } from "../../shared/gameAction";

@Component({
    selector: 'game-action-list',
    templateUrl: './game-action-list.component.html',
    styleUrls: ['./game-action-list.component.scss']
})
/** GameActionList component*/
export class GameActionListComponent implements OnInit {
  /** GameActionList ctor */
    constructor(private gameService: GameService, private route: ActivatedRoute) {
    }

  ngOnInit(): void {
    this.gameId = this.route.snapshot.params["gameId"];
    this.gameService.getGameActionForGame(this.gameId)
      .subscribe(next => {
        this.gameActions = next.json();
        this.gameActions.map(ga => {
          if (ga.picture !== null) ga.picture.imageData = 'data:image/png;base64,' + ga.picture.image;
        });
      });
  }
  public isNaN(value): boolean {
    return "NaN" === value;
  }

  gameId: number;
  gameActions: GameAction[];
}
