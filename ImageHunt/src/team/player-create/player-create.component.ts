import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Player } from '../../shared/player';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'player-create',
  templateUrl: './player-create.component.html',
  styleUrls: ['./player-create.component.scss']
})
/** player-create component*/
export class PlayerCreateComponent implements OnInit {
  ngOnInit(): void {
    
  }
  @Output() playerCreated = new EventEmitter<Player>();

  /** player-create ctor */
  constructor(public bsModalRef: BsModalRef) {

  }
  createPlayer(form: NgForm) {
    let player: Player = { id: 0, name: this.name, chatLogin: this.chatLogin, startDate: null };
    this.playerCreated.emit(player);
  }

  name: string;
  chatLogin: string;
}
