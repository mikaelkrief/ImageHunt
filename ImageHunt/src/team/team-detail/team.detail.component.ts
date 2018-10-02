import { Component, OnInit } from '@angular/core';
import {TeamService} from "../../shared/services/team.service";
import { NgForm } from '@angular/forms';
import {Team} from "../../shared/team";
import { ActivatedRoute } from "@angular/router";
import {Player} from "../../shared/player";
import { ConfirmationService } from "primeng/api";
import { BsModalService } from 'ngx-bootstrap';
import { PlayerCreateComponent } from '../player-create/player-create.component';

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
  constructor(private _route: ActivatedRoute,
    private _teamService: TeamService,
    private _confirmationService: ConfirmationService,
    private _modalService: BsModalService) {
    this.team = new Team();
  }

    /** Called by Angular after team-detail component initialized */
    ngOnInit(): void {
      this.teamId = this._route.snapshot.params["teamId"];
      this.gameId = this._route.snapshot.params["gameId"];
      this.getTeam(this.teamId);
  }
  getTeam(teamId: number) {
    this._teamService.getTeam(teamId)
      .subscribe((team:Team) => this.team = team);
  }
  createPlayer() {
    this.modalRef = this._modalService.show(PlayerCreateComponent, { ignoreBackdropClick: true });
    this.modalRef.content.playerCreated.subscribe((player: Player) => {
      this._teamService.addMemberToTeam(this.teamId, player).subscribe(() => {
        this.getTeam(this.team.id);
      });
    });
  }
  //  addPlayer(teamId: number, form: NgForm): void {
  //    var player: Player = {id:0, name : form.value.name, chatLogin : form.value.chatId, startDate:null};
  //    this._teamService.addMemberToTeam(teamId, player)
  //      .subscribe(null, null, () => {
  //        this.getTeam(this.team.id);
  //        form.resetForm();
  //      });
  //}
  deletePlayer(teamId:number, player: Player) {
    this._confirmationService.confirm({
      message: "Voulez-vous vraiment retirer ce joueur de sa team ?",
      accept: () => this._teamService.removePlayerToTeam(teamId, player)
        .subscribe(() => this.getTeam(this.team.id))
    });
  }

  teamId: number;
  gameId: number;
  modalRef;
}
