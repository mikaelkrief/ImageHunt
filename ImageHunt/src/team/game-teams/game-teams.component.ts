import { Component, OnInit } from '@angular/core';
import { Game } from '../../shared/game';
import { ActivatedRoute } from '@angular/router';
import { GameService } from 'services/game.service';
import { TeamService } from 'services/team.service';
import { AlertService } from "services/alert.service";
import { BsModalService } from 'ngx-bootstrap';
import { TeamCreateComponent } from "../team-create/team-create.component";
import { Team } from '../../shared/team';

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
  createTeam() {
    this.modalRef = this._modalService.show(TeamCreateComponent, { ignoreBackdropClick: true });
    this.modalRef.content.teamCreated.subscribe((team: Team) => {
      this._teamService.createTeam(this.game.id, team).subscribe((createdTeam: Team) => {
        this.game.teams.push(createdTeam);
      });
    });

  }

  modalRef;
}
