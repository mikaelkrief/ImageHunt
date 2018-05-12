import { Component, OnInit, Input } from '@angular/core';
import {Team} from "../../shared/team";
import {TeamService} from "../../shared/services/team.service";
import { ConfirmationService } from "primeng/api";
//import 'rxjs/add/operator/subscribe';

@Component({
  selector: 'team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.scss']
})
/** team component*/
export class TeamListComponent implements OnInit {
  @Input() teams: Team[];
  @Input() gameId: number;

  /** team ctor */
  constructor(private teamService: TeamService,
    private confirmationService: ConfirmationService) { }

  /** Called by Angular after team component initialized */
  ngOnInit(): void {
  }

  deleteTeam(teamId: number) {
    console.log("deleteTeam");
    this.confirmationService.confirm({
      message: 'Voulez-vous vraiment effacer cette équipe ?',
      accept: () => 
        this.teamService.deleteTeam(teamId)
          .subscribe(() => this.teams.splice(this.teams.indexOf(this.teams.find(t => t.id === teamId)), 1))
    });
  }
}
