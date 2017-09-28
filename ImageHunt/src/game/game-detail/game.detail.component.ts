import { Component, OnInit, TemplateRef, ViewChild, ContentChild, ContentChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {Game} from "../../shared/game";
import {GameService } from "../../shared/services/game.service";
import { NgForm } from "@angular/forms";
import {TeamService} from "../../shared/services/team.service";
import { Team } from "../../shared/team";
import 'rxjs/Rx';
import { BsModalService, BsModalRef } from "ngx-bootstrap";
import {NodeRelation} from "../../shared/NodeRelation";

@Component({
    selector: 'game-detail',
    templateUrl: './game.detail.component.html',
    styleUrls: ['./game.detail.component.scss']
})
/** gameDetail component*/
export class GameDetailComponent implements OnInit
{
  @ContentChildren('fileInput') fileInput;
  @ViewChild('mapComponent') mapComponent;
    public uploadModalRef: BsModalRef;
    game: Game;
  nodeRelations: NodeRelation[];
    /** gameDetail ctor */
    constructor(private _route: ActivatedRoute,
      private _gameService: GameService,
      private _teamService: TeamService,
      private _modalService: BsModalService) {
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
    this._gameService.centerMap(gameId).subscribe(null, null, () => this.getGame(gameId));
  }
  nodeMode(nodeType:string) {
    
  }
  public modalRef: BsModalRef;
  currentLatitude: number;
  currentLongitude: number;

  mapClicked(event, template) {
    var coordinates = event.coords;
    this.currentLatitude = coordinates.lat;
    this.currentLongitude = coordinates.lng;
    this.modalRef = this._modalService.show(template, { ignoreBackdropClick: true });
    
  }

  createNode(form: NgForm) {
    var node = {
      nodeType: form.value.nodeType,
      name: form.value.name,
      latitude: this.currentLatitude,
      longitude: this.currentLongitude
    };
    this._gameService.addNode(this.game.id, node)
      .subscribe(() => this.getGame(this.game.id));

  }

}
