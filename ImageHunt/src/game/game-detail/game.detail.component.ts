import { Component, OnInit, TemplateRef, ViewChild, ContentChild, ContentChildren } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Game } from '../../shared/game';
import { GameService } from '../../shared/services/game.service';
import { NgForm } from '@angular/forms';
import { TeamService } from '../../shared/services/team.service';
import { Team } from '../../shared/team';
import 'rxjs/Rx';
import { BsModalService, BsModalRef, TabsetComponent } from 'ngx-bootstrap';
import { NodeRelation } from '../../shared/NodeRelation';
import { Node } from '../../shared/node';
import { NodeCreateComponent } from '../node-create/node.create.component';
import { NodeEditComponent } from '../node-edit/node-edit.component';
import { NodeRelationComponent } from '../node-relation/node.relation.component';
import { NodeListComponent } from '../node-list/node.list.component';
import { NodeRequest } from '../../shared/nodeRequest';
import { GeoPoint } from '../../shared/GeoPoint';
import { GeoVector } from '../../shared/GeoVector';
import {AlertService} from '../../shared/services/alert.service';
import { forkJoin } from 'rxjs';
import { EditedRelation } from '../../shared/EditedRelation';
import {QuestionNodeComponent} from '../question-node/question.node.component';
import {ConfirmationService} from 'primeng/components/common/confirmationservice';
import { NodeClicked } from "../../shared/NodeClicked";
import { MenuItem } from "primeng/api";
import {RelationClicked} from "../../shared/RelationClicked";
import { ImageNodeEditComponent } from '../image-node-edit/image-node-edit.component';
import { NodeDragged } from '../../shared/NodeDragged';
import { map } from 'rxjs/operators';
import { NodeResponse } from '../../shared/nodeResponse';
import { BatchNodeComponent } from "../batch-node/batch-node.component";

@Component({
  selector: 'game-detail',
  templateUrl: './game.detail.component.html',
  styleUrls: ['./game.detail.component.scss']
})
/** gameDetail component*/
export class GameDetailComponent implements OnInit {
  @ContentChildren('fileInput') fileInput;
  @ViewChild('mapComponent') mapComponent;


  alerts: any = [];
  public uploadModalRef: BsModalRef;
  game: Game;
  nodes: NodeResponse[];
  nodeRelations: NodeRelation[];
  newRelations: GeoVector[];
  currentZoom: number;
  /** gameDetail ctor */
  constructor(private _route: ActivatedRoute,
    private _gameService: GameService,
    private _teamService: TeamService,
    private _modalService: BsModalService,
    private _alertService: AlertService,
    private _confirmationService: ConfirmationService) {
    this.game = new Game();
  }

  /** Called by Angular after gameDetail component initialized */
  ngOnInit(): void {
    let gameId = this._route.snapshot.params['gameId'];
    this.game.id = gameId;
    this.getGame(gameId);
  }
  uploadImages(template: TemplateRef<any>) {
    this.uploadModalRef = this._modalService.show(template);
  }
  uploadKml(template: TemplateRef<any>) {
    this.uploadModalRef = this._modalService.show(template);
  }

  uploadFiles(files) {
    this._gameService.upload(files, this.game.id).subscribe(res => {
      this.uploadModalRef.hide();
      this.getGame(this.game.id);
    }, error => {
      this.uploadModalRef.hide();
      let errdata = error.json();
      this._alertService.sendAlert(`Une des images n'a pu être téléchargée ${errdata.filename}`, "danger", 5000);
    });
  }
  reverse: boolean;
  uploadKmlFiles(files) {
    this._gameService.uploadKml(files[0], this.game.id, this.reverse).subscribe(res => {
      this.uploadModalRef.hide();
      this.getGame(this.game.id);
    }, error => {
      this.uploadModalRef.hide();
      let errdata = error.json();
      this._alertService.sendAlert(`Le fichier Kml n'a pas pu être téléchargé ${errdata.filename}`, "danger", 5000);
    });
  }
  getGame(gameId: number) {
    forkJoin([
        this._gameService.getGameById(gameId),
        this._gameService.getNodeRelations(gameId)])
      .subscribe(responses => {
        this.game = <Game>(responses[0]);
        this.currentZoom = this.game.mapZoom;
        this.nodes = <NodeResponse[]>(responses[1]);
        this.mapComponent.gameId = this.game.id;
        this.mapComponent.nodeRelations = this.nodeRelations;
        this.mapComponent.latCenter= this.game.mapCenterLat;
        this.mapComponent.lngCenter = this.game.mapCenterLng;
        this.mapComponent.zoom = this.game.mapZoom;
        this.mapComponent.nodes = this.game.nodes;
          this.mapComponent.updateMap();
        }
      );
  }

  centerMap(gameId: number) {
    this._gameService.centerMap(gameId).subscribe(null, null,
      () => {
        this._alertService.sendAlert("Centrage de la map effectué", "success", 1000);
        this._gameService.setZoom(gameId, this.currentZoom == undefined || this.currentZoom === 0 ? this.game.mapZoom : this.currentZoom)
          .subscribe(() => this.getGame(gameId));
      });
    
  }
  public modalRef: BsModalRef;
  currentLatitude: number;
  currentLongitude: number;

  mapClicked(event) {

    this.currentLatitude = event.latlng.lat;
    this.currentLongitude = event.latlng.lng;
    let node = new Node();
    node.latitude = this.currentLatitude;
    node.longitude = this.currentLongitude;
    this.modalRef = this._modalService.show(NodeEditComponent, { ignoreBackdropClick: true, class: 'modal-lg', initialState: { node: node } });
    this.modalRef.content.nodeEmit.subscribe(newNode => this.createNode(newNode));

  }

  createNode(node: NodeRequest) {
    this._gameService.addNode(this.game.id, node)
      .subscribe(() => {
        this.getGame(this.game.id);
        this._alertService.sendAlert(`Le noeud ${node.name} à bien été ajouté à la partie`, 'success', 5000);
      });

  }
  nodeDragged(nodeDragged: NodeDragged) {
    this._gameService.updateNode(nodeDragged.node)
      .subscribe(() => this.getGame(this.game.id));
  }

  //nodeClicked(nodeClicked: NodeClicked) {
  //  if (nodeClicked.numberClicked === 1) {
  //    if (nodeClicked.node.nodeType === 'LastNode') {
  //      this.mapComponent.resetNodeClick();
  //      this._alertService.sendAlert(`Le noeud ${nodeClicked.node.name} ne peut pas accepter d'enfant`, 'danger', 5000);
  //      return;
  //    }
  //    if (nodeClicked.node.nodeType === 'QuestionNode') {
  //      this.mapComponent.resetNodeClick();
  //      this._alertService.sendAlert(`Editez les relations des noeuds Question dans le module d'édition des réponses aux questions`, 'danger', 5000);
  //      return;
  //    }
  //    if ((nodeClicked.node.nodeType === 'FirstNode' ||
  //        nodeClicked.node.nodeType === 'TimerNode' ||
  //        nodeClicked.node.nodeType === 'ImageNode' ||
  //        nodeClicked.node.nodeType === 'WaypointNode' ||
  //        nodeClicked.node.nodeType === 'ObjectNode') &&
  //      nodeClicked.node.children.length > 0) {
  //      this.mapComponent.resetNodeClick();
  //      this._alertService.sendAlert(`Le noeud ${nodeClicked.node.name} ne peut pas accepter d'avantage d'enfants`, 'danger', 5000);

  //    }
      
  //  }
  //}

  newRelation(nodeRelation: NodeRelation) {
    var parentNode = this.game.nodes.find(n => n.id === nodeRelation.nodeId);
    var childNode = this.game.nodes.find(n => n.id === nodeRelation.childNodeId);
    if (childNode.nodeType === 'FirstNode') {
      this._alertService.sendAlert(`Le noeud ${childNode.name} ne peut pas être un enfant.`, 'danger', 10000);
      return;
    }
    if (parentNode.nodeType === 'QuestionNode' || parentNode.children.length === 0) {
      if (this.newRelations == null)
        this.newRelations = new Array<GeoVector>();
      this.newRelations.push({
        orgId: parentNode.id,
        org: { latitude: parentNode.latitude, longitude: parentNode.longitude },
        destId: childNode.id,
        dest: { latitude: childNode.latitude, longitude: childNode.longitude }
      });
      this._alertService.sendAlert(`Liaison de ${parentNode.name} vers ${childNode.name} réussie`, 'success', 5000);
    } else if (parentNode.children.length !== 0) {
      this._alertService.sendAlert(`Le noeud ${parentNode.name} ne peut plus accepter d'enfant.`, 'danger', 10000);
      return;
    }
  }
  editNodeRelations() {
    this.modalRef = this._modalService.show(NodeRelationComponent, { ignoreBackdropClick: true, animated: true, class: 'modal-lg'});
    this.modalRef.content.nodes = this.game.nodes;
    this._modalService.onHidden.subscribe(() => {
      this.mapComponent.clearMap();

      this.getGame(this.game.id);
    });
  }
  editNodeAnswers() {
    this.modalRef = this._modalService.show(QuestionNodeComponent, { ignoreBackdropClick: true });
    this.modalRef.content.nodes = this.game.nodes;
    this.modalRef.content.gameId = this.game.id;
  }
  mapZoomChange(zoom) {
    this.currentZoom = zoom;
  }
  saveChanges(gameId: number) {
    if (this.newRelations !== null) {
      this.newRelations.forEach(r => forkJoin(this._gameService.addRelation(r.orgId, r.destId, 0))
        .subscribe(
          
        () => {
          this._alertService.sendAlert("Enregistrement des changements effectué", "success", 1000);
          this.getGame(gameId);
        }));
    }
  }
  teamsUpdated() {
    this._teamService.getTeams(this.game.id)
      .subscribe((teams:Team[]) => this.game.teams = teams);
  }
  batchEdit() {
    this.modalRef = this._modalService.show(BatchNodeComponent, { ignoreBackdropClick: true });
    this.modalRef.content.game = this.game;
  }
  deleteNode(node: Node) {
    this._gameService.deleteNode(node.id)
      .subscribe(res=>this.getGame(this.game.id));
  }

  editNode(node: Node) {
      this._modalService.onHide.subscribe(reason => this.getGame(this.game.id));
    this.modalRef = this._modalService.show(NodeEditComponent, { ignoreBackdropClick: true, class: 'modal-lg', initialState: { node: node } });
      this.modalRef.content.nodeEmit.subscribe(node => this._gameService.updateNode(this.game.id, node)
        .subscribe(() => {
          this.mapComponent.clearMap();
          this.getGame(this.game.id);
        }));
  }
}
