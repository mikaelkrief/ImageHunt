import { Component, OnInit } from '@angular/core';
import { GameService } from 'services/game.service';
import { ActivatedRoute } from '@angular/router';
import { Passcode } from '../../shared/Passcode';
import { environment } from '../../environments/environment';

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
    this.botName = environment.BOT_NAME;
  }
  ngOnInit(): void {
    this.gameId = this._route.snapshot.params['gameId'];
    this._gamerService.getPasscodesForGame(this.gameId)
      .subscribe((passcodes: Passcode[]) => this.passcodes = passcodes);
    this._gamerService.getGameById(this.gameId)
      .subscribe(game => this.game = game);
  }

  gameId;
  passcodes: Passcode[];
  botName:string;
  game;
}
