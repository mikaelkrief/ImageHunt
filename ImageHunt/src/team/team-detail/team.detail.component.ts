import { Component, OnInit } from '@angular/core';
import {TeamService} from "../../shared/services/team.service";
import { NgForm } from '@angular/forms';
import {Team} from "../../shared/team";
import { ActivatedRoute } from "@angular/router";
import {Player} from "../../shared/player";
import { ConfirmationService } from "primeng/api";

@Component({
    selector: 'team-detail',
    templateUrl: './team.detail.component.html',
    styleUrls: ['./team.detail.component.scss']
})
/** team-detail component*/
export class TeamDetailComponent implements OnInit
{
  team: Team;
    /** team-detail ctor */
  constructor(private _route: ActivatedRoute, private _teamService: TeamService, private _confirmationService: ConfirmationService) {
    this.team = new Team();
  }

    /** Called by Angular after team-detail component initialized */
    ngOnInit(): void {
      let teamId = this._route.snapshot.params["teamId"];
      this.getTeam(teamId);
  }
  getTeam(teamId: number) {
    this._teamService.getTeam(teamId)
      .subscribe(next => this.team = next.json());
  }
    addPlayer(teamId: number, form: NgForm): void {
      var player: Player = {id:0, name : form.value.name, chatLogin : form.value.chatId, startDate:null};
      this._teamService.addMemberToTeam(teamId, player)
        .subscribe(null, null, () => {
          this.getTeam(this.team.id);
          form.resetForm();
        });
  }
  deletePlayer(teamId:number, player: Player) {
    this._confirmationService.confirm({
      message: "Vouslez-vous vraiment retirer ce joueur de sa team ?",
      accept: () => this._teamService.removePlayerToTeam(teamId, player)
        .subscribe(() => this.getTeam(this.team.id))
    });
  }
}
