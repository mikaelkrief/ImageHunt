import { Injectable } from "@angular/core";
import { JwtHttp } from "ng2-ui-auth";
import { Game } from "../game";
import { Node } from "../node";
import { Http, RequestOptions, Headers } from "@angular/http";
import {NodeRequest} from "../nodeRequest";
import { Observable } from "rxjs/Observable";
import {QuestionNodeAnswerRelation} from "../QuestionNodeAnswerRelation";

@Injectable()
export class GameService {
  constructor(private http: Http,
    private jwtHttp: JwtHttp) { }
  getGameForAdmin(adminId: number) {
    return this.jwtHttp.get('api/Game/ByAdminId/' + adminId);
  }
  getGameById(gameId: number) {
    return this.jwtHttp.get('api/Game/byId/' + gameId).map(g => g.json());
  }
  createGame(adminId: number, game: Game) {
    return this.jwtHttp.post('api/Game/' + adminId, game);
  }
  deleteGame(gameId: number) {
    return this.jwtHttp.delete('api/Game/' + gameId);
  }
  addNode(gameId: number, node: NodeRequest) {
    return this.jwtHttp.post(`api/Game/AddNode/${gameId}`, node);
  }
  upload(files: File[], gameId) {
    let headers = new Headers();
    headers.delete('Content-Type');
    const formData = new FormData();
    for (var file of files) {
      formData.append("files", file);
    }
    let options = new RequestOptions({ headers: headers });

    return this.http.put(`api/Game/AddPictures/${gameId}`, formData, options);
  }

  centerMap(gameId: number) {
    return this.jwtHttp.post(`api/Game/CenterGameByNodes/${gameId}`, null);

  }

  getNodeRelations(gameId: number) {
     return this.jwtHttp.get(`api/Game/NodesRelations/${gameId}`);
  }

  addRelation(orgNodeId: number, destNodeId: number, answerId: number) {

    return this.jwtHttp.post('api/Node/AddRelationToNode', { nodeId: orgNodeId, childrenId: destNodeId, answerId: answerId });
    //return this.jwtHttp.post('api/node/AddRelationToNode', null);

  }
  removeRelation(orgNodeId: number, destNodeId: number) {
    return this.jwtHttp.post("api/Node/RemoveRelationToNode", { nodeId: orgNodeId, childrenId: destNodeId, answerId: 0 });
  }

  setZoom(gameId: number, zoom: number) { return this.jwtHttp.patch(`api/Game/UpdateZoom/${gameId}/${zoom}`, null); }
  getQuestionNodesOfGame(gameId: number)  {
    return this.jwtHttp.get(`api/Game/GetQuestionNodeOfGame/${gameId}`).map(j=>j.json());
  }
  addRelationAnswers(relations: QuestionNodeAnswerRelation[]) {
    return this.jwtHttp.post(`api/Node/AddRelationsWithAnswers`, relations);
  }
  getGameActionForGame(gameId: number) {
    return this.jwtHttp.get(`api/Game/GetGameActions/` + gameId);
  }
  getGameAction(gameActionId: number) {
    return this.jwtHttp.get(`api/Game/GetGameAction/` + gameActionId);
  }
}
