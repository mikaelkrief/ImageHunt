import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import {Globals} from "../../shared/globals";
import {Admin} from "../../shared/admin";
import { Game } from '../../shared/game';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    selector: 'game-create',
    templateUrl: './game.create.component.html',
    styleUrls: ['./game.create.component.scss']
})
/** game.create component*/
export class GameCreateComponent implements OnInit
{
  @Output() game = new EventEmitter<Game>();
  admin: Admin;
  name: string;
  startDate: string;
  startTime: string;
    /** game.create ctor */
  constructor(private globals: Globals, public bsModalRef: BsModalRef) { }

    /** Called by Angular after game.create component initialized */
    ngOnInit(): void {
      this.admin = this.globals.connectedUser;
  }
  uploadPicture() {

  }
  createGame() {
    let game: Game = {
      id: 0,
      isActive: true,
      name: this.name,
      startDate: new Date(this.startDate + ' ' + this.startTime),
      mapCenterLng: 0,
      mapCenterLat: 0,
      mapZoom: 1,
      nodes: [],
      teams: []
    };
    this.game.emit(game);
    this.name = '';
    this.startDate = undefined;
    this.startTime = undefined;
  }
}
