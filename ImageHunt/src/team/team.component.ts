import { Component, OnInit } from '@angular/core';
import {Team } from './Team';
import {TeamService} from './team.service';

@Component({
    selector: 'app-team',
    templateUrl: './team.component.html',
    styleUrls: ['./team.component.scss']
})
/** team component*/
export class TeamComponent implements OnInit
{
    /** team ctor */
    constructor(private teamService: TeamService) {
      
    }

    /** Called by Angular after team component initialized */
    ngOnInit(): void {
      this.teamService.getTeams()
        .then(t => this.teams = t);
      //this.teams = [
      //  { id: 1, name: "team1", players: [{ name: "player1" }, { name: "player2" }, { name: "player3" }] },
      //  { id: 2, name: "team2", players: [{ name: "player4" }] },
      //];
    }

  teams: Team[];
}
