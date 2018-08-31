import { Component, OnInit } from '@angular/core';
import { GameService } from 'services/game.service';
import { ActivatedRoute } from '@angular/router';
import { Passcode } from '../../shared/Passcode';

@Component({
  selector: 'passcode-print',
  templateUrl: './passcode-print.component.html',
  styleUrls: ['./passcode-print.component.scss']
})
/** passcode-print component*/
export class PasscodePrintComponent implements OnInit {
  /** passcode-print ctor */
  constructor(private _gamerService: GameService,
              private _route: ActivatedRoute) {

  }
  ngOnInit(): void {
    this.gameId = this._route.snapshot.params['gameId'];
    this._gamerService.getPasscodesForGame(this.gameId)
      .subscribe((passcodes: Passcode[]) => this.passcodes = passcodes);
  }

  gameId;
  passcodes: Passcode[];
}
