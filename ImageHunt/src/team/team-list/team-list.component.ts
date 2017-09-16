import { Component, OnInit } from '@angular/core';
import {Team} from "../../shared/team";
import {TeamService} from "../../shared/services/team.service";
//import 'rxjs/add/operator/subscribe';

@Component({
  selector: 'team',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.scss']
})
/** team component*/
export class TeamListComponent implements OnInit {
  teams: Team[];
  /** team ctor */
  constructor(private _teamService: TeamService) { }

  /** Called by Angular after team component initialized */
  ngOnInit(): void {
    this._teamService.getTeams()
      .subscribe(next => this.teams = next.json(),
      err => console.error(err.status));
  }
}
