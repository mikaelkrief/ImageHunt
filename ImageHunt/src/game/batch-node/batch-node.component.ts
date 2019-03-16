import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { GameService } from 'services/game.service';
import { NgForm } from '@angular/forms';
import { Game } from 'shared/game';
import { AlertService } from 'services/alert.service';

@Component({
    selector: 'batch-node',
    templateUrl: './batch-node.component.html',
    styleUrls: ['./batch-node.component.scss']
})
/** batch-node component*/
export class BatchNodeComponent {
    /** batch-node ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService, private _alertService: AlertService) {

  }
  game: Game;
  proceed(form: NgForm) {
    const multiplier = form.value.multiplier;
    const updaterType = form.value.updaterType;
    const nodeType = form.value.nodeType;
    const seedPattern = form.value.nodeUpdaterPattern;
    // double the anti-slash of seed pattern
    //const st = JSON.stringify(seedPattern);
    const argument = `--seedPattern=${seedPattern} --nodeType=${nodeType} --multiplier=${multiplier}`;
    this._gameService.batchUpdate(this.game.id, updaterType, argument)
      .subscribe(res => {},
        err => this._alertService.sendAlert(`Error while applying updater ${updaterType}: ${err}`, "danger", 10000));
  }
}
