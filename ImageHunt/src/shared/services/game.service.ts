import { Injectable } from '@angular/core';
import { Game } from '../game';
import { Node } from '../node';
import { Http, RequestOptions, Headers } from '@angular/http';
import { HttpParams } from '@angular/common/http';
import {NodeRequest} from '../nodeRequest';
import { Observable } from 'rxjs';
import {QuestionNodeAnswerRelation} from '../QuestionNodeAnswerRelation';

@Injectable()
export class GameService {
  constructor(private http: Http) { }
  getGameForAdmin(adminId: number) {
    return this.http.get('api/Game/ByAdminId/' + adminId);
  }
  getGameById(gameId: number) {
    return this.http.get('api/Game/byId/' + gameId).map(g => g.json());
  }
  createGame(adminId: number, game: Game) {
    return this.http.post('api/Game/' + adminId, game);
  }
  deleteGame(gameId: number) {
    return this.http.delete('api/Game/' + gameId);
  }
  addNode(gameId: number, node: NodeRequest) {
    return this.http.post(`api/Game/AddNode/${gameId}`, node);
  }
  deleteNode(nodeId: number) {
    return this.http.delete(`api/Node/RemoveNode/${nodeId}`);
  }
  upload(files: File[], gameId) {
    let headers = new Headers();
    headers.delete('Content-Type');
    const formData = new FormData();
    for (var file of files) {
      formData.append('files', file);
    }
    let options = new RequestOptions({ headers: headers });

    return this.http.put(`api/Game/AddPictures/${gameId}`, formData, options);
  }

  centerMap(gameId: number) {
    return this.http.put(`api/Game/CenterGameByNodes/${gameId}`, null);

  }

  getNodeRelations(gameId: number) {
     return this.http.get(`api/Game/NodesRelations/${gameId}`).map(r=>r.json());
  }

  addRelation(orgNodeId: number, destNodeId: number, answerId: number) {

    return this.http.put('api/Node/AddRelationToNode', { nodeId: orgNodeId, childrenId: destNodeId, answerId: answerId });
    //return this.http.put('api/node/AddRelationToNode', null);

  }
  removeRelation(orgNodeId: number, destNodeId: number) {
    return this.http.delete(`api/Node/RemoveRelation/${orgNodeId}/${destNodeId}`);
  }

  setZoom(gameId: number, zoom: number) { return this.http.patch(`api/Game/UpdateZoom/${gameId}/${zoom}`, null); }
  getQuestionNodesOfGame(gameId: number)  {
    return this.http.get(`api/Game/GetQuestionNodeOfGame/${gameId}`).map(j=>j.json());
  }
  addRelationAnswers(relations: QuestionNodeAnswerRelation[]) {
    return this.http.put(`api/Node/AddRelationsWithAnswers`, relations);
  }
  getGameActionCountForGame(gameId: number) {
    return this.http.get(`api/Game/GameActionCount/${gameId}`);
  }
  getGameActionForGame(gameId: number, pageIndex: number, pageSize: number) {
    var gameActionListRequest = {
      gameId: gameId,
      pageSize: pageSize,
      pageIndex: pageIndex
    }
    return this.http.get(`api/Game/GameActions/`, { params: gameActionListRequest});
  }
  getGameActionsToValidateForGame(gameId: number, pageIndex: number, pageSize: number, nbProbableNode: number) {
    var gameActionListRequest = {
      gameId: gameId,
      take: pageSize,
      pageIndex: pageIndex,
      nbPotential: nbProbableNode
    }
    return this.http.get(`api/Game/GameActionsToValidate/`, { params: gameActionListRequest});
  }
  getGameAction(gameActionId: number) {
    return this.http.get(`api/Game/GetGameAction/${gameActionId}`);
  }
  validateGameAction(gameActionId: number) {
    return this.http.put(`api/Action/Validate/${gameActionId}`, null);
  }

  getGameReviewed() { return this.http.get("api/Game/Reviewed"); }
  getScoreForGame(gameId) {
     return this.http.get(`api/Game/Score/${gameId}`);
  }
  getPicturesNodes(gameId: number) {
    return this.http.get(`api/Game/GetPictureNodes/${gameId}`);
  }
  getNodeById(nodeId: number) {
    return this.http.get(`api/Node/${nodeId}`);
  }
  updateNode(node: Node) {
    const nodeRequest = {
      id: node.id,
      latitude: node.latitude,
      longitude: node.longitude,
      name: node.name,
      points: node.points,
    };
    return this.http.patch(`api/Node/`, nodeRequest);
  }
  getGameActionsForGame(gameId: number) {
    return this.http.get(`api/Action/${gameId}`);
  }
}
