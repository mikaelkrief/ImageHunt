import { Injectable } from "@angular/core";
import { Http } from "@angular/http";

@Injectable()
export class GameService {
  constructor(private http: Http) {  }
  getGameById(gameId: number) {
    return this.http.get('api/game/' + gameId).map(g=>g.json());
  }
}
