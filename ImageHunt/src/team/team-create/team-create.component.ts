import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TeamService } from "../../shared/services/team.service";
import {Team} from "../../shared/team";

@Component({
    selector: 'team-create',
    templateUrl: './team-create.component.html',
    styleUrls: ['./team-create.component.scss']
})
/** teamCreate component*/
export class TeamCreateComponent implements OnInit {
  teamCreateForm: FormGroup;
  @Input() gameId: number;
  @Output() teamCreated = new EventEmitter();
    /** teamCreate ctor */
    constructor(private fb: FormBuilder, private teamService: TeamService) {
    }
  ngOnInit(): void {
    this.teamCreateForm = this.fb.group({
      teamName: ['', [Validators.required, Validators.minLength(5)]],
      teamColor: ['']
    });

    }
  hasFormErrors() {
    return !this.teamCreateForm.valid;
  }
  fieldErrors(field: string) {
    let controlState = this.teamCreateForm.controls[field];
    return (controlState.dirty && controlState.errors) ? controlState.errors : null;
  }
  teamColor: any;
  createTeam() {
    let teamName = this.teamCreateForm.value['teamName'];
    let team: Team = { id: 0, name: teamName, players: null };
    this.teamService.createTeam(this.gameId, team)
      .subscribe(() => {
        this.teamCreated.emit();
        this.teamCreateForm.reset();
      });

  }
}
