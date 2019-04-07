import { Component, OnInit } from '@angular/core';
import { LiveService } from '../../shared/services/live.service';
import { ActivatedRoute } from '@angular/router';
import { Team } from '../../shared/team';
import { TeamPosition } from '../../shared/teamPosition';
import * as L from 'leaflet';
import { Game } from '../../shared/game';
import { GameService } from '../../shared/services/game.service';
import { GameAction } from '../../shared/gameAction';
import { forkJoin } from 'rxjs';


@Component({
  selector: 'team-follow',
  templateUrl: './team-follow.component.html',
  styleUrls: ['./team-follow.component.scss']
})
/** team-follow component*/
export class TeamFollowComponent implements OnInit {
  /** team-follow ctor */
  constructor(private _liveService: LiveService, private _gameService: GameService, private route: ActivatedRoute) {

    this._liveService._connection.on("ActionSubmitted",
      (gameAction) => this.handleNewPosition(gameAction));

  }
  getColorForTeam(team: Team) {
    if (team.color === '')
      return '#' + Math.floor(Math.random() * 16777215).toString(16);
    else
      return team.color;
  }
  handleNewPosition(gameAction) {
    if (gameAction.game.id === this.gameId) {
      this.handleGameAction(gameAction);
    }
  }

  handleGameAction(gameAction: GameAction) {
    if (!gameAction.team)
      return;
    if (!this.positions.has(gameAction.team.id)) {
      this.positions.set(gameAction.team.id, new Array<GameAction>());
      let color = this.getColorForTeam(gameAction.team);
      gameAction.team.color = color;
      let polyline = new L.Polyline(
        [[gameAction.latitude, gameAction.longitude], [gameAction.latitude, gameAction.longitude]],
        { color: color });
      polyline.addTo(this.pathLayer);
      this.paths.set(gameAction.team.id, polyline);
    }
    let pos = this.positions.get(gameAction.team.id);
    pos.push(gameAction);
    let poly = this.paths.get(gameAction.team.id);
    poly.addLatLng([gameAction.latitude, gameAction.longitude]);
    this.createMarker(gameAction);
  }

  //createMarker(gameAction: GameAction) {
  //  let icon;
  //  let iconUrl;
  //  switch (gameAction.action) {
  //    case 0:
  //      iconUrl = 'assets/startNode.png';
  //      break;
  //    case 1:
  //      iconUrl = 'assets/endNode.png';
  //      break;
  //    case 2:
  //      iconUrl = 'assets/pictureNode.png';
  //      break;
  //    case 4:
  //      iconUrl = 'assets/questionNode.png';
  //      break;
  //    case 5:
  //      iconUrl = 'assets/objectNode.png';
  //      break;
  //    default:
  //      break;
  //  }
  //  if (iconUrl !== undefined) {
  //    icon = new L.Icon({
  //      iconUrl: iconUrl,
  //      iconSize: [32, 32],
  //      iconAnchor: [16, 16]
  //    });

  //    const marker = new L.Marker([gameAction.latitude, gameAction.longitude], { icon: icon, draggable: false });
  //    marker.addTo(this.markersLayer);
  //  }
  //}
  createMarker(gameAction: GameAction) {
    let icon;
    let iconClass;
    switch (gameAction.action) {
      case 0:
        iconClass = {
          icon: 'flag',
          prefix: 'fa',
          markerColor: 'red'
        };
        break;
      case 1:
        iconClass = {
          icon: 'flag-checkered',
          prefix: 'fa',
          markerColor: 'green'
        };
        break;
      case 2:
        iconClass = {
          icon: 'camera',
          prefix: 'fa',
          markerColor: 'blue'
        };
        break;
      case 3:
        iconClass = {
          icon: 'fa-map-marker-alt',
          prefix: 'fa',
          markerColor: 'blue'
        };
        break;
      case 4:
        iconClass = {
          icon: 'question-circle',
          prefix: 'fa',
          markerColor: 'blue'
        };
        break;
      case 5:
        iconClass = {
          icon: 'running',
          prefix: 'fa',
          markerColor: 'darkred'
        };
      case 10:
        iconClass = {
          icon: 'gift',
          prefix: 'fa',
          markerColor: 'darkpurple'
        };
      case 11:
        iconClass = {
          icon: 'mask',
          prefix: 'fa',
          markerColor: 'purple'
        };
        break;
      default:
        break;
    }
    icon = L.AwesomeMarkers.icon(iconClass);
      const marker = new L.Marker([gameAction.latitude, gameAction.longitude], { icon: icon, draggable: false });
      marker.addTo(this.markersLayer);
    }


  ngOnInit(): void {
    this.map = L.map("map").setView([0, 0], 12);
    L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
      attribution: 'ImageHunt'
    }).addTo(this.map);


    this.gameId = +this.route.snapshot.params["gameId"];
    forkJoin<Game, GameAction[]>(
      this._gameService.getGameById(this.gameId),
      this._gameService.getGameActionsForGame(this.gameId)
    )
      .subscribe(([game, gameActions]) => {
        this.game = game;
        this.map.setView([this.game.mapCenterLat, this.game.mapCenterLng], this.map.zoom);
        this.pathLayer = new L.LayerGroup();
        this.pathLayer.addTo(this.map);
        this.markersLayer = new L.LayerGroup();
        this.markersLayer.addTo(this.map);
        L.control.layers(null, { "Trajets": this.pathLayer, "Actions": this.markersLayer }).addTo(this.map);
        this.retrievePositions(gameActions);
        this._liveService._connection.start()
          .then(() => {
            console.log("Connection to SignalR successfull");
            this._liveService._connection.send("InitConnection");
          })
          .catch(error => console.error(error));

      });
  }
  retrievePositions(gameActions: GameAction[]) {
    for (let gameAction of gameActions) {
      this.handleGameAction(gameAction);
    }
    this.map.fitBounds();
  }
  paths: Map<number, L.Polyline> = new Map<number, L.Polyline>();

  positions: Map<number, Array<GameAction>> = new Map<number, Array<GameAction>>();

  gameId: number;
  map: any;
  game: Game;
  pathLayer: L.LayerGroup<any>;
  markersLayer: L.LayerGroup<any>;
}
