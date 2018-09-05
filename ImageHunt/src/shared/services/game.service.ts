import { Injectable } from '@angular/core';
import { Game } from '../game';
import { Node } from '../node';
import { Http, RequestOptions, Headers } from '@angular/http';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import {NodeRequest} from '../nodeRequest';
import { Observable } from 'rxjs';
import {QuestionNodeAnswerRelation} from '../QuestionNodeAnswerRelation';
import { GameAction } from '../gameAction';
import { Passcode } from '../Passcode';

@Injectable()
export class GameService {
  constructor(private http: HttpClient) { }
  getGameForAdmin(adminId: number) {
    return this.http.get('api/Game/ByAdminId/' + adminId);
  }
  getGameById(gameId: number) {
    return this.http.get('api/Game/byId/' + gameId);
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
    let headers = new HttpHeaders();
    headers.delete('Content-Type');
    const formData = new FormData();
    for (var file of files) {
      formData.append('files', file);
    }
    let options = { headers: headers };

    return this.http.put(`api/Game/AddPictures/${gameId}`, formData, options);
  }

  centerMap(gameId: number) {
    return this.http.put(`api/Game/CenterGameByNodes/${gameId}`, null);

  }

  getNodeRelations(gameId: number) {
     return this.http.get(`api/Game/NodesRelations/${gameId}`);
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
    return this.http.get(`api/Game/GetQuestionNodeOfGame/${gameId}`);
  }
  addRelationAnswers(relations: QuestionNodeAnswerRelation[]) {
    return this.http.put(`api/Node/AddRelationsWithAnswers`, relations);
  }
  getGameActionCountForGame(gameId: number) {
    return this.http.get(`api/Game/GameActionCount/${gameId}`);
  }
  getGameActionForGame(gameId: number, pageIndex: number, pageSize: number) {

    return this.http.get<GameAction[]>(`api/Game/GameActions/${gameId}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
  }
  getGameActionsToValidateForGame(gameId: number, pageIndex: number, pageSize: number, nbProbableNode: number) {
    return this.http.get<GameAction[]>(`api/Game/GameActionsToValidate/${gameId}&pageIndex=${pageIndex}&pageSize=${pageSize}&nbPotential=${nbProbableNode}`);
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

  getPasscodesForGame(gameId: number) {
    return this.http.get(`api/Passcode/${gameId}`);
  }

  deletePasscode(gameId: number, passcode: Passcode) {
    return this.http.delete(`api/Passcode/gameId=${gameId}&passcodeId=${passcode.id}`);
  }
  addPasscode(gameId: number, passcode: Passcode) {
    let request =
    {
        gameId: gameId,
        pass: passcode.pass,
        nbRedeem: passcode.nbRedeem,
        points: passcode.points
    }
    return this.http.post(`api/Passcode/`, request);
  }
}
