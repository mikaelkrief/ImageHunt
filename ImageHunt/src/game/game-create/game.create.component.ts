import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { Globals } from "../../shared/globals";
import { Admin } from "../../shared/admin";
import { Game } from '../../shared/game';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { UploadImageComponent } from '../../shared/upload-image/upload-image.component';

@Component({
  selector: 'game-create',
  templateUrl: './game.create.component.html',
  styleUrls: ['./game.create.component.scss']
})
/** game.create component*/
export class GameCreateComponent implements OnInit {
  uploadModalRef: BsModalRef;
  @Output() game = new EventEmitter<Game>();
  admin: Admin;
  name: string;
  startDate: string;
  startTime: string;
  pictureId?: number;
  description: string;
  /** game.create ctor */
  constructor(private globals: Globals, public bsModalRef: BsModalRef, private _modalService: BsModalService) { }

  /** Called by Angular after game.create component initialized */
  ngOnInit(): void {
    this.admin = this.globals.connectedUser;
  }
  modalRef;

  uploadPicture() {
    this.modalRef = this._modalService.show(UploadImageComponent, { ignoreBackdropClick: true });
    this.modalRef.content.pictureId.subscribe(id => this.pictureId = id);
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
      teams: [],
      pictureId: this.pictureId,
      description: this.description
    };
    this.game.emit(game);
    this.name = '';
    this.startDate = undefined;
    this.startTime = undefined;
  }
}
