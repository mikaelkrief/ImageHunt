import { Component, OnInit } from '@angular/core';
import { Game } from '../../shared/game';
import { ActivatedRoute } from '@angular/router';
import { GameService } from 'services/game.service';
import { TeamService } from 'services/team.service';
import { AlertService } from "services/alert.service";
import { BsModalService } from 'ngx-bootstrap';

@Component({
    selector: 'game-teams',
    templateUrl: './game-teams.component.html',
    styleUrls: ['./game-teams.component.scss']
})
/** game-teams component*/
export class GameTeamsComponent implements OnInit {
  ngOnInit(): void {
    const gameCode = this._route.snapshot.params['gameCode'];
    this._gameService.getGameByCode(gameCode)
      .subscribe(g => this.game = g);

  }
  game: Game;
    /** game-teams ctor */
  constructor(private _route: ActivatedRoute,
    private _gameService: GameService,
    private _teamService: TeamService,
    private _modalService: BsModalService,
    private _alertService: AlertService, ) {
    }
}
