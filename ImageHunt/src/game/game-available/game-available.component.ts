import { Component, OnInit } from '@angular/core';
import { Game } from '../../shared/game';
import { GameService } from 'services/game.service';

@Component({
    selector: 'game-available',
    templateUrl: './game-available.component.html',
    styleUrls: ['./game-available.component.scss']
})
/** game-available component*/
export class GameAvailableComponent implements OnInit {
    ngOnInit(): void {
      this._gameService.getAllGame()
        .subscribe(games => this.games = games);
    }
  games: Game[];
    /** game-available ctor */
    constructor(private _gameService: GameService) {

  }
  joinGame(game: Game) {

  }
}
