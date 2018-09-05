import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TeamService } from "../../shared/services/team.service";
import {Team} from "../../shared/team";
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    selector: 'team-create',
    templateUrl: './team-create.component.html',
    styleUrls: ['./team-create.component.scss']
})
/** teamCreate component*/
export class TeamCreateComponent implements OnInit {
  teamCreateForm: FormGroup;
  @Input() gameId: number;
  @Output() teamCreated = new EventEmitter<Team>();
    /** teamCreate ctor */
  constructor(public bsModalRef: BsModalRef, private teamService: TeamService) {
    }
  ngOnInit(): void {
    }
  hasFormErrors() {
    return !this.teamCreateForm.valid;
  }
  fieldErrors(field: string) {
    let controlState = this.teamCreateForm.controls[field];
    return (controlState.dirty && controlState.errors) ? controlState.errors : null;
  }
  createTeam() {
    let team: Team = { id: 0, name: this.name, players: null, color:this.color };
    this.teamCreated.emit(team);
    this.teamCreateForm.reset();

  }
  color: string;
  name:string;
}
