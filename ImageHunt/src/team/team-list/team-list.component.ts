import { Team } from "../../shared/team";
import { TeamService } from "../../shared/services/team.service";
import { ConfirmationService } from "primeng/api";
import { TeamCreateComponent } from "../team-create/team-create.component";

@Component({
  selector: "team-list",
  templateUrl: "./team-list.component.html",
  styleUrls: ["./team-list.component.scss"]
})
/** team component*/
export class TeamListComponent implements OnInit {
  teams: Team[];
  gameId: number;

  /** team ctor */
  constructor(private teamService: TeamService,
    private confirmationService: ConfirmationService,
    private _modalService: BsModalService,
    private route: ActivatedRoute) {
  }

  /** Called by Angular after team component initialized */
  ngOnInit(): void {
    this.gameId = this.route.snapshot.params["gameId"];
    this.teamService.getTeams(this.gameId)
      .subscribe(teams => this.teams = teams);
  }

  deleteTeam(teamId: number) {
    this.confirmationService.confirm({
      message: "Voulez-vous vraiment effacer cette équipe ?",
      accept: () => this.teamService.deleteTeam(teamId)
        .subscribe(() => this.teams.splice(this.teams.indexOf(this.teams.find(t => t.id === teamId)), 1))
    });

  }

  createTeam() {
    this.modalRef = this._modalService.show(TeamCreateComponent, { ignoreBackdropClick: true });
    this.modalRef.content.teamCreated.subscribe((team: Team) => {
      this.teamService.createTeam(this.gameId, team).subscribe((createdTeam: Team) => {
        this.teams.push(createdTeam);
      });
    });

  }

  modalRef;
}
