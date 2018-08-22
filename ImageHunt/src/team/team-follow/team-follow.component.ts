import { Component, OnInit } from '@angular/core';
import { LiveService } from '../../shared/services/live.service';
import { ActivatedRoute } from '@angular/router';
import { Team } from '../../shared/team';
import { TeamPosition } from '../../shared/teamPosition';
import * as L from 'leaflet';
import { Game } from '../../shared/game';
import { GameService } from '../../shared/services/game.service';
import { GameAction } from '../../shared/gameAction';

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

    this._liveService._connection.start()
      .then(() => {
        console.log("Connection to SignalR successfull");
        this._liveService._connection.send("InitConnection");
      })
      .catch(error => console.error(error));
  }
  handleNewPosition(gameAction) {
    if (gameAction.game.id === this.gameId) {
      if (!this.positions.has(gameAction.team.id)) {
        this.positions.set(gameAction.team.id, new Array<GameAction>());
        let polyline = new L.Polyline([[gameAction.latitude, gameAction.longitude], [gameAction.latitude, gameAction.longitude]],
          { color: '#' + Math.floor(Math.random() * 16777215).toString(16) });
        polyline.addTo(this.map);
        this.paths.set(gameAction.team.id, polyline);
      }
      let pos = this.positions.get(gameAction.team.id);
      pos.push(gameAction);
      let poly = this.paths.get(gameAction.team.id);
      poly.addLatLng([gameAction.latitude, gameAction.longitude]);
    }
  }
  ngOnInit(): void {
    this.map = L.map("map").setView([0, 0], 12);
    L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
      attribution: 'ImageHunt'
    }).addTo(this.map);


    this.gameId = +this.route.snapshot.params["gameId"];
    this._gameService.getGameById(this.gameId).subscribe(res => {
      this.game = res;
      this.map.setView([this.game.mapCenterLat, this.game.mapCenterLng], this.map.zoom);
    });
  }
  retrievePositions(gameId: number) {
    //this._gameService.getGameActionForGame()
  }
  paths: Map<number, L.Polyline> = new Map<number, L.Polyline>();

  positions: Map<number, Array<GameAction>> = new Map<number, Array<GameAction>>();

  gameId: number;
  map: any;
  game: Game;
}
