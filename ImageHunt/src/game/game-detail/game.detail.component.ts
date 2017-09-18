import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {Game} from "../../shared/game";
import {GameService } from "../../shared/services/game.service";
import { NgForm } from "@angular/forms";
import {TeamService} from "../../shared/services/team.service";
import {Player} from "../../shared/player";
import { Team } from "../../shared/team";
import { Node } from "../../shared/node";

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
    constructor(private _route: ActivatedRoute,
      private _gameService: GameService,
      private _teamService: TeamService) {
      this.game = new Game();
    }

    /** Called by Angular after gameDetail component initialized */
    ngOnInit(): void {
      let gameId = this._route.snapshot.params["gameId"];
      this._gameService.getGameById(gameId).subscribe(res => {
        this.game = res;
      });
    }
  createTeam(gameId: number, form: NgForm) {
    var team: Team = { id: 0, name: form.value.name, players: null };
    this._teamService.createTeam(gameId, team)
      .subscribe(null, null, () => {
        this._gameService.getGameById(gameId).subscribe(res => this.game = res);
        form.resetForm();
      });
  }
  addMysteryPicture(gameId: number) {
    var node: Node;
    this._gameService.addNode(gameId, null);
  }

}
