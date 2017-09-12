import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {Game} from "../../shared/game";
import {GameService } from "../../shared/services/game.service";

@Component({
    selector: 'game-detail',
    templateUrl: './game.detail.component.html',
    styleUrls: ['./game.detail.component.scss']
})
/** gameDetail component*/
export class GameDetailComponent implements OnInit
{
  game:Game;
    /** gameDetail ctor */
    constructor(private _route: ActivatedRoute, private _gameService: GameService) { }

    /** Called by Angular after gameDetail component initialized */
    ngOnInit(): void {
      let gameId = this._route.snapshot.params["gameId"];
      this._gameService.getGameById(gameId).subscribe(res => this.game = res);
    }
}
