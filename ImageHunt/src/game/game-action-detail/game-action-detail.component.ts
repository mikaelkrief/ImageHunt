import { Component, OnInit } from '@angular/core';
import {GameService} from "../../shared/services/game.service";
import {ActivatedRoute} from "@angular/router";
import {GameAction} from "../../shared/gameAction";
import { Team } from 'shared/team';
import { AlertService } from 'services/alert.service';

@Component({
    selector: 'game-action-detail',
    templateUrl: './game-action-detail.component.html',
    styleUrls: ['./game-action-detail.component.scss']
})
/** game-action-detail component*/
export class GameActionDetailComponent implements OnInit {
  actionTypes;
  ngOnInit(): void {
    this.actionId = this.route.snapshot.params["gameActionId"];
    this.gameService.getGameAction(this.actionId)
      .subscribe((next: GameAction) => {
        this.gameAction = next;
        this.computeDeltaForProbableNodes();
        this.selectedNode = this.gameAction.probableNodes[0];
        //this.gameAction.picture.imageData = 'data:image/png;base64,' + this.gameAction.picture.image;
        this.setMap();
      });
    
  }
  /** game-action-detail ctor */
  constructor(private gameService: GameService, private route: ActivatedRoute, private alertService: AlertService) {
    this.gameAction = new GameAction();
    this.gameAction.latitude = 0;
    this.gameAction.longitude = 0;
    this.gameAction.team = new Team();
    this.setMapCenter();
    this.actionTypes = [
      { label: 'Start game', value: 0 },
      { label: 'End game', value: 1 },
      { label: 'Submit picture', value: 2 },
      { label: 'Visit waypoint', value: 3 },
      { label: 'Reply Question', value: 4 },
      { label: 'Do action', value: 5 },
      { label: 'Submit position', value: 6 },
      { label: 'Redeem passcode', value: 7 },
      { label: 'Give points', value: 8 },
      { label: 'Node', value: 9 },
      { label: 'Visit Bonus node', value: 10 },
      { label: 'Visit Hidden node', value: 11 },
      ];
  }
  setMapCenter() {
    this.mapCenter = { lat: this.gameAction.latitude, lng: this.gameAction.longitude };
    this.options = {
      center: { lat: this.mapCenter.lat, lng: this.mapCenter.lng },
      zoom: 12
    };
  }

  options: any;

  overlays: any[];

  actionId: number;
  gameAction: GameAction;
  mapOptions: any;
  mapCenter: { lat: number;lng: number };

  setMap() {

  }
  selectNode(node) {
    this.selectedNode = node;
    this.computeDelta();
  }
  validate(gameAction: GameAction) {
    this.gameService.validateGameAction(gameAction.id, this.selectedNode.id)
      .subscribe(next => {
        this.gameAction.isValidated = next.isValidated;
        this.gameAction.isReviewed = next.isReviewed;
        this.gameAction.pointsEarned = next.pointsEarned;
      },
        error => this.handleError(error));
  }
  reject(action: GameAction) {
    this.gameService.rejectGameAction(action.id).subscribe(next => {
        action.isValidated = false;
        action.isReviewed = true;
        action.pointsEarned = 0;
      },
      error => this.handleError(error));
  }
  handleError(error): void {
    switch (error.status) {
    case 401:
      this.alertService.sendAlert("You are not authorized to validate player's actions", "danger", 10000);
    default:
    }
  }

  deg2rad(deg) {
    return deg * (Math.PI / 180);
  }

  getDistanceFromLatLon(lat1, lon1, lat2, lon2) {
    var R = 6371000; // Radius of the earth in km
    var dLat = this.deg2rad(lat2 - lat1);  // deg2rad below
    var dLon = this.deg2rad(lon2 - lon1);
    var a =
        Math.sin(dLat / 2) * Math.sin(dLat / 2) +
          Math.cos(this.deg2rad(lat1)) * Math.cos(this.deg2rad(lat2)) *
          Math.sin(dLon / 2) * Math.sin(dLon / 2)
      ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c; // Distance in m
    return d;
  }
  computeDeltaForProbableNodes() {
    this.gameAction.probableNodes.map(node => node.delta = this.getDistanceFromLatLon(this.gameAction.latitude,
      this.gameAction.longitude,
      node.latitude,
      node.longitude));
  }
  computeDelta() {
    this.gameAction.delta = this.getDistanceFromLatLon(this.gameAction.latitude,
          this.gameAction.longitude,
          this.selectedNode.latitude,
        this.selectedNode.longitude);
  }

  next(gameAction) {
    this.gameService.nextGameAction(gameAction.game.id, gameAction.id)
      .subscribe(next => {
        this.gameAction = next;
        this.computeDeltaForProbableNodes();
        this.selectNode(this.gameAction.probableNodes[0]);
      });
  }
  setPoints(action: GameAction) {
    this.gameService.modifyGameAction(action)
      .subscribe(res => {
          action.isValidated = res.isValidated;
          action.isReviewed = res.isReviewed;
          action.pointsEarned = res.pointsEarned;
        },
        error => this.handleError(error));
  }

  selectedNode;
}
