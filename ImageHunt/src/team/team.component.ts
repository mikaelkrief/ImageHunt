import { Component, OnInit } from '@angular/core';
import { TeamService } from "./team.service";
import { Team } from "../shared/team";
//import 'rxjs/add/operator/subscribe';

@Component({
  selector: 'team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})
/** team component*/
export class TeamComponent implements OnInit {
  teams: Team[];
  /** team ctor */
  constructor(private _teamService: TeamService) { }

  /** Called by Angular after team component initialized */
  ngOnInit(): void {
    this._teamService.getTeams()
      .subscribe(res => this.teams = res,
      err => console.error(err.status));
  }
}
