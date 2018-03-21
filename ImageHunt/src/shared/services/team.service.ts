import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import { JwtHttp } from "ng2-ui-auth";
import {Player} from "../player";
import {Team} from "../team";

@Injectable()
export class TeamService {
  constructor(private http: JwtHttp) { }
  getTeams() {
    return this.http.get('api/Team');

  }
  getTeam(teamId) {
    return this.http.get('api/Team/' + teamId);
  }
  addMemberToTeam(teamId: number, player: Player) {
    return this.http.put('api/Team/' + teamId, player);
  }
  createTeam(gameId: number, team: Team) {
    return this.http.post('api/Team/' + gameId, team);
  }
  deleteTeam(teamId: number) {
    return this.http.delete('api/Team/' + teamId);
  }
}
