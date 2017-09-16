import { Component, OnInit } from '@angular/core';
import {TeamService} from "../../shared/services/team.service";
import {Team} from "../../shared/team";
import { ActivatedRoute } from "@angular/router";
import {Player} from "../../shared/player";

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
  constructor(private _route: ActivatedRoute, private _teamService: TeamService) {
    this.team = new Team();
  }

    /** Called by Angular after team-detail component initialized */
    ngOnInit(): void {
      let teamId = this._route.snapshot.params["teamId"];
      this._teamService.getTeam(teamId)
        .subscribe(next => this.team = next.json());
  }
    addPlayer(teamId: number, playerName: string, playerChatId: string): void {
      var player: Player = <Player>({name : playerName, chatLogin : playerChatId});
      this._teamService.addMemberToTeam(teamId, player).subscribe();
    }
}
