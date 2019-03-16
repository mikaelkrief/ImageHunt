import { Component, OnInit } from '@angular/core';
import { GameService } from 'services/game.service';
import { ActivatedRoute } from '@angular/router';
import { Game } from '../../shared/game';
import { Passcode } from '../../shared/Passcode';
import { forkJoin } from 'rxjs';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { PasscodeCreateComponent } from '../passcode-create/passcode-create.component';

@Component({
    selector: 'passcode-list',
    templateUrl: './passcode-list.component.html',
    styleUrls: ['./passcode-list.component.scss']
})
/** passcode-list component*/
export class PasscodeListComponent implements OnInit {
    /** passcode-list ctor */
  constructor(private _route: ActivatedRoute, private _gameService: GameService, private _modalService: BsModalService) {

  }
  ngOnInit(): void {
    let gameId = this._route.snapshot.params['gameId'];
    this.gameId = gameId;
    this.getGame(gameId);
  }

  game: Game;
  public modalRef: BsModalRef;

  getGame(gameId) {
    this._gameService.getGameById(gameId).subscribe((game: Game) => this.game = game);
    this._gameService.getPasscodesForGame(gameId).subscribe((passcodes: Passcode[]) => this.passcodes = passcodes);
  }

  passcodes: Passcode[];
  gameId;
  deletePasscode(passcode: Passcode) {
    this._gameService.deletePasscode(this.gameId, passcode).subscribe(() => {
      this.passcodes.splice(this.passcodes.indexOf(this.passcodes.find(t => t.id === passcode.id)), 1);
    });
  }
  createPassode() {
    this.modalRef = this._modalService.show(PasscodeCreateComponent, { ignoreBackdropClick: true });
    this.modalRef.content.gameId = this.gameId;
    this.modalRef.content.newPasscode.subscribe(passcode => this.createPasscode(passcode));


  }

  createPasscode(passcode: Passcode) {
    this._gameService.addPasscode(this.gameId, passcode)
      .subscribe((pass:Passcode) => this.passcodes.push(pass));
  }
  printPasscodes() {

  }
}
