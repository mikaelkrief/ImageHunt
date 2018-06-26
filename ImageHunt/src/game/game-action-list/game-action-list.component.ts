import { Component, OnInit } from '@angular/core';
import {GameService} from "../../shared/services/game.service";
import { ActivatedRoute } from "@angular/router";
import { GameAction } from "../../shared/gameAction";
import { LazyLoadEvent } from 'primeng/api';

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

    this.gameService.getGameActionCountForGame(this.gameId)
      .subscribe(next => {
        this.totalRecords = next.json();
      });
  }
  loadData(event: LazyLoadEvent) {  
    this.gameService.getGameActionForGame(this.gameId, (event.first / event.rows) + 1, event.rows)
      .subscribe(next => {
        this.gameActions = next.json();
        this.totalRecords = 15;
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
  totalRecords: number;
}
