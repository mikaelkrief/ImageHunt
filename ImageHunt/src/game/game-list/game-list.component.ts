import { Component, OnInit } from '@angular/core';
import {GameService} from "../../shared/services/game.service";
import {Game} from "../../shared/game";
import { LocalStorageService } from "angular-2-local-storage";
import {Admin} from "../../shared/admin";
import { DatePipe } from '@angular/common';
import { NgForm } from "@angular/forms";

@Component({
    selector: 'game-list',
    templateUrl: './game-list.component.html',
    styleUrls: ['./game-list.component.scss']
})
/** game-list component*/
export class GameListComponent implements OnInit
{
  games: Game[];
  minDate: Date;
  admin:Admin;
    /** game ctor */
    constructor(private gameService: GameService, private localStorageService: LocalStorageService) { }

    /** Called by Angular after game component initialized */
    ngOnInit(): void {
      this.minDate = new Date();
      this.admin = <Admin>(this.localStorageService.get('connectedAdmin'));
      this.getGames();
    }

  getGames() {
    if (this.admin != null)
      this.gameService.getGameForAdmin(this.admin.id).subscribe(next => this.games = next.json());
  }

  createGame(form: NgForm) {
    var startDate = <Date>(form.value.date);
    startDate.setTime(form.value.time);
      var game: Game = { id: 0, name: form.value.name, startDate: startDate, isActive: true };
    this.gameService.createGame(this.admin.id, game)
      .subscribe(null, null, () => {
        this.getGames();
        form.resetForm();
      });
  }
  classForActive(active: boolean) {
    return active ? 'fa-eye' : 'fa-eye-slash';
  }
}
