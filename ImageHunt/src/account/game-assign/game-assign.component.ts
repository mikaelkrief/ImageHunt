import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { GameService } from 'services/game.service';
import { AdminService } from 'services/admin.service';
import { User } from 'shared/user';
import { Game } from "shared/game";

@Component({
    selector: 'game-assign',
    templateUrl: './game-assign.component.html',
    styleUrls: ['./game-assign.component.scss']
})
/** game-assign component*/
export class GameAssignComponent implements OnInit {
  user: User;
  selectedGames: Game[] = [{ id: 1, name: "first", isActive: true, isPublic: true }];
  games: Game[];
  notSelectedGames: Game[] = [{ id: 2, name: "second", isActive: true, isPublic: true }];

    /** game-assign ctor */
  constructor(public bsModalRef: BsModalRef,
    private _gameService: GameService,
    private _adminService: AdminService) {

  }
  ngOnInit(): void {
    this._gameService.getAllGame()
      .subscribe(games => {
        this.games = games;
        this.selectedGames = this.games.filter(g => this.user.games.find(ga => ga.id === g.id));
        this.notSelectedGames = this.games.filter(g => !(this.user.games.find(ga => ga.id === g.id)));
      });
  }
  saveChanges() {
    this.notSelectedGames = this.games.filter(g => !this.selectedGames.find(ga =>ga.id === g.id));
    for (var gameS of this.selectedGames) {
      this._adminService.assignGame(this.user.appUserId, gameS.id, true)
        .subscribe();
    }
    for (var gameN of this.notSelectedGames) {
      this._adminService.assignGame(this.user.appUserId, gameN.id, false)
        .subscribe();
    }
    this.bsModalRef.hide();
  }

}
