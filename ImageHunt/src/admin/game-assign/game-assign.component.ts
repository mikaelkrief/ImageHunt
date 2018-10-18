import { Component, Input, OnInit } from '@angular/core';
import { Admin } from '../../shared/admin';
import { BsModalRef } from 'ngx-bootstrap';
import { Game } from '../../shared/game';
import { GameService } from 'services/game.service';

@Component({
  selector: 'game-assign',
  templateUrl: './game-assign.component.html',
  styleUrls: ['./game-assign.component.scss']
})
/** game-assign component*/
export class GameAssignComponent implements OnInit {
  admin: Admin;
  selectedGames: Game[] = [{id:1, name:"first", isActive:true}];
  games: Game[];
  notSelectedGames: Game[] = [{ id: 2, name: "second", isActive: true }];;
  /** game-assign ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService) {

  }
  ngOnInit(): void {
    this._gameService.getAllGame()
      .subscribe(games => {
        this.games = games;
        this.selectedGames = this.games.filter(g => this.admin.gameIds.includes(g.id));
        this.notSelectedGames = this.games.filter(g => !(this.admin.gameIds.includes(g.id)));
      });
  }

}
