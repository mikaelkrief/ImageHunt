import { Component, OnInit } from '@angular/core';
import {GameService} from "../../shared/services/game.service";
import {ActivatedRoute} from "@angular/router";
import {GameAction} from "../../shared/gameAction";
import { } from '@types/googlemaps';
@Component({
    selector: 'game-action-detail',
    templateUrl: './game-action-detail.component.html',
    styleUrls: ['./game-action-detail.component.scss']
})
/** game-action-detail component*/
export class GameActionDetailComponent implements OnInit {
  ngOnInit(): void {
    this.actionId = this.route.snapshot.params["gameActionId"];
    this.gameService.getGameAction(this.actionId)
      .subscribe(next => {
        this.gameAction = next.json();
        this.setMap();
      });
    
  }
  /** game-action-detail ctor */
  constructor(private gameService: GameService, private route: ActivatedRoute) {
    this.gameAction = new GameAction();
    this.gameAction.latitude = 0;
    this.gameAction.longitude = 0;
    this.setMapCenter();
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
    var lineSymbol = {
      path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
    };
    this.options = {
      center: { lat: this.gameAction.latitude, lng: this.gameAction.longitude },
      zoom: 12
    };
    this.overlays = [
      new google.maps.Marker({ position: { lat: this.gameAction.latitude, lng: this.gameAction.longitude }, title: "Localisation" }),
      new google.maps.Circle({ center: { lat: this.gameAction.latitude, lng: this.gameAction.longitude }, fillColor: '#1976D2', fillOpacity: 0.35, strokeWeight: 1, radius: 40 }),
    ];
    if (this.gameAction.node != null) {
      this.overlays.push(new google.maps.Marker({
        position: { lat: this.gameAction.node.latitude, lng: this.gameAction.node.longitude },
        title: this.gameAction.node.name
      }));
      this.overlays.push(new google.maps.Circle({
        center: { lat: this.gameAction.node.latitude, lng: this.gameAction.node.longitude },
        fillColor: '#1976D2',
        fillOpacity: 0.35,
        strokeWeight: 1,
        radius: 40
      }));
    }

  }
}
