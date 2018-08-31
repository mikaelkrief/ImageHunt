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

  createPasscode(form: NgForm) {
    let passcode: Passcode = { id: 0, pass: form.value.pass, nbRedeem: form.value.nbRedeem, points: form.value.points };
    this.newPasscode.emit(passcode);
  }
}
