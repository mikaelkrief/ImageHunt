import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, NgForm, FormControl } from "@angular/forms";
import { TeamService } from "../../shared/services/team.service";
import {Team} from "../../shared/team";
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { UploadImageComponent } from '../../shared/upload-image/upload-image.component';


@Component({
    selector: 'team-create',
    templateUrl: './team-create.component.html',
    styleUrls: ['./team-create.component.scss']
})
/** teamCreate component*/
export class TeamCreateComponent implements OnInit {
  @Input() gameId: number;
  @Output() teamCreated = new EventEmitter<Team>();
    /** teamCreate ctor */
  constructor(public bsModalRef: BsModalRef,
    private teamService: TeamService, private _modalService: BsModalService) {
    }
  ngOnInit(): void {
    }
  createTeam() {
    let team: Team = {
      id: 0, name: this.teamGroup.value.name, players: null,
      color: this.teamGroup.value.color,
      cultureInfo: this.teamGroup.value.language,
      picture: {Id:this.pictureId},
    };
    this.teamCreated.emit(team);
    //this.teamCreateForm.reset();

  }
  teamGroup = new FormGroup({
    name: new FormControl(''),
    language: new FormControl('Fr-fr'),
    color: new FormControl('')
  });
  //color: string;
  //name:string;
  //language: string;
  modalRef;
  pictureId?: number;

  uploadPicture() {
  this.modalRef = this._modalService.show(UploadImageComponent, { ignoreBackdropClick: true });
  this.modalRef.content.pictureId.subscribe(id => this.pictureId = id);
}

}
