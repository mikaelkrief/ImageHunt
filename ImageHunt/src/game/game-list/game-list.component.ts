import { Component, OnInit } from '@angular/core';
import { GameService } from "../../shared/services/game.service";
import { Game } from "../../shared/game";
import { LocalStorageService } from "angular-2-local-storage";
import { Admin } from "../../shared/admin";
import { DatePipe } from '@angular/common';
import { NgForm } from "@angular/forms";
import { AlertService } from "../../shared/services/alert.service";
import { ConfirmationService } from "primeng/api";
import { BsModalService } from 'ngx-bootstrap';
import { GameCreateComponent } from '../game-create/game.create.component';

@Component({
  selector: 'game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.scss']
})
/** game-list component*/
export class GameListComponent implements OnInit {
  games: Game[];
  minDate: Date;
  today: Date;
  /** game ctor */
  constructor(private gameService: GameService,
    private localStorageService: LocalStorageService,
    private _alertService: AlertService,
    private _confirmationService: ConfirmationService,
    private _modalService: BsModalService) {
    this.today = new Date();
  }

  /** Called by Angular after game component initialized */
  ngOnInit(): void {
    this.minDate = new Date();
    this.getGames();
  }
  showModal() {
    this.modalRef = this._modalService.show(GameCreateComponent, { ignoreBackdropClick: true });
    this.modalRef.content.game.subscribe(game => this.createGame(game));
  }
  getGames() {
      this.gameService.getGameForConnectedUser()
        .subscribe((games: Game[]) => this.games = games,
          err => this._alertService.sendAlert("Erreur lors de la mise à jour de la liste des jeux", "danger", 10000));
  }

  createGame(game: Game) {
    this.gameService.createGame(game)
      .subscribe(() => {
        this.getGames();
        this._alertService.sendAlert(`La partie ${game.name} a bien été créée`, "success", 5000);
      },
      error => {
        if (error.status == 403) {
          this._alertService.sendAlert(`Vous n'êtes pas autorisé à créer des parties`, "danger", 10000);
        } else {
          this._alertService.sendAlert(`Impossible de créer la partie`, "danger", 10000);
        }
      });
  }
  deleteGame(gameId: number) {
    this._confirmationService.confirm({
      message: "Voulez-vous vraiment effacer cette partie ?",
      accept: () => this.gameService.deleteGame(gameId)
        .subscribe(() => {
          this.getGames();
        },error => this._alertService.sendAlert("Impossible d'effacer la partie", "danger", 10000))
    });
  }
  classForActive(active: boolean) {
    return active ? 'fa-eye' : 'fa-eye-slash';
  }

  modalRef;
}
