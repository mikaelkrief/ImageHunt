import { Component, OnInit } from '@angular/core';
import { GameService } from "../../shared/services/game.service";
import { Game } from "../../shared/game";
import { LocalStorageService } from "angular-2-local-storage";
import { Admin } from "../../shared/admin";
import { DatePipe } from '@angular/common';
import { NgForm } from "@angular/forms";
import {AlertService} from "../../shared/services/alert.service";
import { ConfirmationService } from "primeng/api";

@Component({
  selector: 'game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.scss']
})
/** game-list component*/
export class GameListComponent implements OnInit {
  games: Game[];
  minDate: Date;
  admin: Admin;
  /** game ctor */
  constructor(private gameService: GameService,
    private localStorageService: LocalStorageService,
    private _alertService: AlertService,
    private _confirmationService: ConfirmationService) { }

  /** Called by Angular after game component initialized */
  ngOnInit(): void {
    this.minDate = new Date();
    this.admin = <Admin>(this.localStorageService.get('connectedAdmin'));
    this.getGames();
  }

  getGames() {
    if (this.admin != null)
      this.gameService.getGameForAdmin(this.admin.id)
        .subscribe((games:Game[]) => this.games = games, err=> this._alertService.sendAlert("Erreur lors de la mise à jour de la liste des jeux", "danger", 10000));
  }

  createGame(form: NgForm) {
    var startDate = <Date>(form.value.startDate);
    var game: Game = { id: 0, name: form.value.name, startDate: startDate, isActive: true, mapCenterLat: 0, mapCenterLng: 0, mapZoom: 12.0, nodes: null, teams:null };
    this.gameService.createGame(this.admin.id, game)
      .subscribe(null, null, () => {
        this.getGames();
        form.resetForm();
      });
  }
  deleteGame(gameId: number) {
    this._confirmationService.confirm({
      message: "Voulez-vous vraiment effacer cette partie ?",
      accept: () =>this.gameService.deleteGame(gameId)
      .subscribe(null, null, () => {
        this.getGames();
      })
    });
  }
  classForActive(active: boolean) {
    return active ? 'fa-eye' : 'fa-eye-slash';
  }
}
