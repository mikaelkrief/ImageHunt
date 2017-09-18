import { Injectable } from "@angular/core";
import { JwtHttp } from "ng2-ui-auth";
import {Game} from "../game";

@Injectable()
export class GameService {
  constructor(private http: JwtHttp) { }
  getGameForAdmin(adminId: number) {
    return this.http.get('api/game/ByAdminId/' + adminId);
  }
  getGameById(gameId: number) {
    return this.http.get('api/game/byId/' + gameId).map(g=>g.json());
  }
  createGame(adminId: number, game: Game) {
    return this.http.post('api/game/' + adminId, game);
  }
}
