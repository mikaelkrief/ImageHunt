import { Component, OnInit, TemplateRef, ViewChild, ContentChild, ContentChildren } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Game } from "../../shared/game";
import { GameService } from "../../shared/services/game.service";
import { NgForm } from "@angular/forms";
import { TeamService } from "../../shared/services/team.service";
import { Team } from "../../shared/team";
import 'rxjs/Rx';
import { BsModalService, BsModalRef, TabsetComponent } from "ngx-bootstrap";
import { NodeRelation } from "../../shared/NodeRelation";
import { Node } from "../../shared/node";
import { NodeCreateComponent } from "../node-create/node.create.component";
import { NodeRelationComponent } from "../node-relation/node.relation.component";
import { NodeListComponent } from "../node-list/node.list.component";
import { NodeRequest } from "../../shared/nodeRequest";
import { GeoPoint } from "../../shared/GeoPoint";
import { GeoVector } from "../../shared/GeoVector";
import {AlertService} from "../../shared/services/alert.service";
import { Observable } from "rxjs/Observable";
import { EditedRelation } from "../../shared/EditedRelation";
import { ContextMenuComponent} from 'ngx-contextmenu';
import {QuestionNodeComponent} from "../question-node/question.node.component";

@Component({
  selector: 'game-detail',
  templateUrl: './game.detail.component.html',
  styleUrls: ['./game.detail.component.scss']
})
/** gameDetail component*/
export class GameDetailComponent implements OnInit {
  @ContentChildren('fileInput') fileInput;
  @ViewChild('mapComponent') mapComponent;
  @ViewChild(ContextMenuComponent) public basicMenu: ContextMenuComponent;

  alerts: any = [];
  public uploadModalRef: BsModalRef;
  game: Game;
  nodeRelations: NodeRelation[];
  newRelations: GeoVector[];
  currentZoom: number;
  /** gameDetail ctor */
  constructor(private _route: ActivatedRoute,
    private _gameService: GameService,
    private _teamService: TeamService,
    private _modalService: BsModalService,
    private _alertService: AlertService) {
    this.game = new Game();
  }

  /** Called by Angular after gameDetail component initialized */
  ngOnInit(): void {
    let gameId = this._route.snapshot.params["gameId"];
    this.game.id = gameId;
    this.getGame(gameId);
  }
  uploadImages(template: TemplateRef<any>) {
    this.uploadModalRef = this._modalService.show(template);
  }

  uploadFiles(files) {
    this._gameService.upload(files, this.game.id).subscribe(res => {
      this.uploadModalRef.hide();
      this.getGame(this.game.id);
    });
  }
  getGame(gameId: number) {
    this._gameService.getGameById(gameId).subscribe(res => {
      this.game = res;
      this.getNodeRelations(gameId);
    },
      err => console.error("getGame raise error: " + err));
  }
  getNodeRelations(gameId: number) {
    this._gameService.getNodeRelations(gameId)
      .subscribe(res => {
        this.nodeRelations = res.json();
        this.buildRelations();
        this.newRelations = null;
      });
  }
  buildRelations() {
    let nodes = this.game.nodes;
    for (var relation of this.nodeRelations) {
      // Find the origin node
      var orgNode = nodes.find(n => n.id === relation.nodeId);
      for (var childId of relation.childNodeId) {
        orgNode.children.push(nodes.find(n => n.id === childId));
      }
    }
    this.mapComponent.nodes = this.game.nodes;
  }
  createTeam(gameId: number, form: NgForm) {
    var team: Team = { id: 0, name: form.value.name, players: null };
    this._teamService.createTeam(gameId, team)
      .subscribe(null, null, () => {
        this._gameService.getGameById(gameId).subscribe(res => this.game = res);
        form.resetForm();
      });
  }
  centerMap(gameId: number) {
    this._gameService.centerMap(gameId).subscribe(null, null,
      () => {
        this._gameService.setZoom(gameId, this.currentZoom)
          .subscribe(() => this.getGame(gameId));
      });
    
  }
  nodeMode(nodeType: string) {
  }
  public modalRef: BsModalRef;
  currentLatitude: number;
  currentLongitude: number;

  mapClicked(event) {

    var coordinates = event.coords;
    this.currentLatitude = coordinates.lat;
    this.currentLongitude = coordinates.lng;
    this.modalRef = this._modalService.show(NodeCreateComponent, { ignoreBackdropClick: true });
    this.modalRef.content.latitude = this.currentLatitude;
    this.modalRef.content.longitude = this.currentLongitude;
    this.modalRef.content.newNode.subscribe(node => this.createNode(node));

  }

  createNode(node: NodeRequest) {
    this._gameService.addNode(this.game.id, node)
      .subscribe(() => this.getGame(this.game.id));

  }
  nodeClicked(node: Node) {
    if (node == this.mapComponent.firstNode) {
      if (node.nodeType === "LastNode") {
        this.mapComponent.resetNodeClick();
        this._alertService.sendAlert(`Le noeud ${node.name} ne peut pas accepter d'enfant`, "danger", 5000);
        return;
      }
      if (node.nodeType === "QuestionNode") {
        this.mapComponent.resetNodeClick();
        this._alertService.sendAlert(`Editez les relations des noeuds Question dans le module d'édition des réponses aux questions`, "danger", 5000);
        return;
      }
      if ((node.nodeType === "FirstNode" ||
          node.nodeType === "TimerNode" ||
          node.nodeType === "ImageNode" ||
          node.nodeType === "ObjectNode") &&
        node.children.length > 0) {
        this.mapComponent.resetNodeClick();
        this._alertService.sendAlert(`Le noeud ${node.name} ne peut pas accepter d'avantage d'enfants`, "danger", 5000);

      }
      
    }
  }
  newRelation(nodeRelation: NodeRelation) {
    var parentNode = this.game.nodes.find(n => n.id === nodeRelation.nodeId);
    var childNode = this.game.nodes.find(n => n.id === nodeRelation.childNodeId[0]);
    if (childNode.nodeType === "FirstNode") {
      this._alertService.sendAlert(`Le noeud ${childNode.name} ne peut pas être un enfant.`, "danger", 10000);
      return;
    }
    if (parentNode.nodeType === "QuestionNode" || parentNode.children.length === 0) {
      if (this.newRelations == null)
        this.newRelations = new Array<GeoVector>();
      this.newRelations.push({
        orgId: parentNode.id,
        org: { latitude: parentNode.latitude, longitude: parentNode.longitude },
        destId: childNode.id,
        dest: { latitude: childNode.latitude, longitude: childNode.longitude }
      });
      this._alertService.sendAlert(`Liaison de ${parentNode.name} vers ${childNode.name} réussie`, "success", 5000);
    } else if (parentNode.children.length !== 0) {
      this._alertService.sendAlert(`Le noeud ${parentNode.name} ne peut plus accepter d'enfant.`, "danger", 10000);
      return;
    }
  }
  editNodeRelations() {
    this.modalRef = this._modalService.show(NodeRelationComponent, { ignoreBackdropClick: true });
    this.modalRef.content.nodes = this.game.nodes;
    this.modalRef.content.editRelations.subscribe(relations => this.saveEditedRelations(relations));
  }
  editNodeAnswers() {
    this.modalRef = this._modalService.show(QuestionNodeComponent, { ignoreBackdropClick: true });
    this.modalRef.content.gameId = this.game.id;
  }
  saveEditedRelations(editedRelations: EditedRelation[]) {
    for (var relation of editedRelations) {
      
    }
  }
  mapZoomChange(zoom) {
    this.currentZoom = zoom;
  }
  saveChanges(gameId: number) {
    this.newRelations.forEach(r => Observable.forkJoin(this._gameService.addRelation(r.orgId, r.destId)).subscribe(() => this.getGame(gameId)));
  }
}
