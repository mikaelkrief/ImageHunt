import { Component, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { NgForm } from '@angular/forms';
import { GameService } from 'services/game.service';
import { Passcode } from '../../shared/Passcode';

@Component({
    selector: 'passcode-create',
    templateUrl: './passcode-create.component.html',
    styleUrls: ['./passcode-create.component.scss']
})
/** passcode-create component*/
export class PasscodeCreateComponent {
    /** passcode-create ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService) {

  }
  public gameId: number;
  @Output() newPasscode = new EventEmitter<Passcode>();
  pass: string;
  nbRedeem: number = -1;
  points: number = 10;

  createPasscode() {
    let passcode: Passcode = { id: 0, pass: this.pass, nbRedeem: this.nbRedeem, points: this.points, qrCode: '' };
    this.newPasscode.emit(passcode);
  }
  generatePassword() {
    this.pass = '';
    for (var i = 0; i < 20; i++) {
      this.pass += Math.random().toString(36).replace(/[^a-z]+/g, '').substr(0, 1);
    }
    this.pass = this.pass.toUpperCase();
  }

}
