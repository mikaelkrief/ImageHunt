import { Component, OnInit } from '@angular/core';
import { LiveService } from '../../shared/services/live.service';
import { ActivatedRoute } from '@angular/router';
import { Team } from '../../shared/team';
import { TeamPosition } from '../../shared/teamPosition';
import * as L from 'leaflet';
import { Game } from '../../shared/game';
import { GameService } from '../../shared/services/game.service';

@Component({
    selector: 'team-follow',
    templateUrl: './team-follow.component.html',
    styleUrls: ['./team-follow.component.scss']
})
/** team-follow component*/
export class TeamFollowComponent implements OnInit {
    /** team-follow ctor */
  constructor(private _liveService: LiveService, private _gameService: GameService, private route: ActivatedRoute) {

    this._liveService._connection.on("PositionChanged",
      (game, team, dateOccured, position) => this.handleNewPosition(game, team, dateOccured, position));

    this._liveService._connection.start()
      .then(() => {
        console.log("Connection to SignalR successfull");
        this._liveService._connection.send("InitConnection");
      })
      .catch(error => console.error(error));
  }
  handleNewPosition(game: Game, team: Team, dateOccured, position: L.LatLng) {
    if (game.id === this.gameId) {
      if (!this.positions.has(team.id)) {
        this.positions.set(team.id, new Array<TeamPosition>());
      }
      let pos = this.positions.get(team.id);
      pos.push({team, dateOccured, position });
    }
  }
  ngOnInit(): void {
    this.map = L.map("map");

    this.gameId = +this.route.snapshot.params["gameId"];
    this._gameService.getGameById(this.gameId).subscribe(res => {
      this.game = res;
      this.map.setView([this.game.mapCenterLat, this.game.mapCenterLng], this.map.zoom);
    });
  }
  positions: Map<number, Array<TeamPosition>> = new Map<number, Array<TeamPosition>>();

  gameId: number;
  map: any;
  game: Game;
}
