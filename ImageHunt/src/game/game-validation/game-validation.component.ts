import { Component, OnInit } from '@angular/core';
import { GameService } from 'services/game.service';
import { JwtHelperService } from "@auth0/angular-jwt";
import { Game } from "shared/game";

@Component({
    selector: 'game-validation',
    templateUrl: './game-validation.component.html',
    styleUrls: ['./game-validation.component.scss']
})
/** game-validation component*/
export class GameValidationComponent implements OnInit {
  ngOnInit(): void {
    const token = localStorage.getItem("authToken");
    const decoded = this._jwtHelperService.decodeToken(token);
    this._gameService.gamesToValidate(decoded.sub)
      .subscribe(games => this.games = games);
  }
    /** game-validation ctor */
  constructor(private _gameService: GameService, private _jwtHelperService: JwtHelperService) {

    }

  games: Game[];
}
