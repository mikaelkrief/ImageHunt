import { Component, OnInit } from '@angular/core';
import {GameService} from "../../shared/services/game.service";
import { ActivatedRoute } from "@angular/router";
import { GameAction } from "../../shared/gameAction";
import { Node } from "../../shared/node";
import { LazyLoadEvent } from 'primeng/api';
import { AlertService } from 'services/alert.service';

@Component({
    selector: 'game-action-list',
    templateUrl: './game-action-list.component.html',
    styleUrls: ['./game-action-list.component.scss']
})
/** GameActionList component*/
export class GameActionListComponent implements OnInit {
  selectedProbableNode: any;
  /** GameActionList ctor */
    constructor(private gameService: GameService, private route: ActivatedRoute, private alertService: AlertService) {
    }
  images: any[][] = [];
  ngOnInit(): void {
    this.gameId = this.route.snapshot.params["gameId"];

    this.gameService.getGameActionCountForGame(this.gameId)
      .subscribe((next: number) => {
        this.totalRecords = next;
      });
  }
  loadData(event: LazyLoadEvent) {  
    this.gameService.getGameActionsToValidateForGame(this.gameId, (event.first / event.rows) + 1, event.rows, 3)
      .subscribe((gameActions: GameAction[]) => {
        this.gameActions = gameActions;
        this.computeDeltas();
      });
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

  deg2rad(deg) {
    return deg * (Math.PI / 180)
  }
  probableNodeChanged(event, action: GameAction) {
    action.node = event.value;
    let selectedNode = event.value as Node;
    action.delta =
      this.getDistanceFromLatLon(action.latitude, action.longitude, selectedNode.latitude, selectedNode.longitude);

  }
  validatedBtnClass(action: GameAction) {
    if (action.isValidated === null)
      return "btn";
    if (action.isValidated)
      return "btn btn-success";
    else
      return "btn btn-danger";
  }
  validatedSpanClass(action: GameAction) {
    if (action.isValidated === null || !action.isValidated)
      return "fa fa-square";
    if (action.isValidated)
      return "fa fa-check-square";
  }
  reviewedSpanClass(action: GameAction) {
    if (action.isReviewed === null || !action.isReviewed)
      return "fa fa-square";
    if (action.isReviewed)
      return "fa fa-check-square";
  }
  validateGameAction(action: GameAction) {
    this.gameService.validateGameAction(action.id)
      .subscribe(next => {
          action.isValidated = true;
          action.isReviewed = true;
          action.pointsEarned = action.node.points;
        },
        error => this.handleError(error)
      );
  }
  rejectGameAction(action: GameAction) {
    this.gameService.rejectGameAction(action.id).subscribe(next => {
      action.isValidated = false;
      action.isReviewed = true;
      action.pointsEarned = 0;
    },
      error => this.handleError(error));
  }
  public isNaN(value): boolean {
    return "NaN" === value;
  }

  gameId: number;
  gameActions: GameAction[];
  totalRecords: number;
  loadBigImage(pictureId) {

  }

  computeDeltas() {
    this.gameActions.map(ga => {
      ga.delta = this.getDistanceFromLatLon(ga.latitude,
        ga.longitude,
        ga.probableNodes[0].latitude,
        ga.probableNodes[0].longitude);
    });
  }

  handleError(error): void {
    switch (error.status) {
    case 401:
        this.alertService.sendAlert("Vous n'\xEAtes pas autoris\xE9 \xE0 valider les actions des joueurs", "danger", 10000);
    default:
    }
  }
}
