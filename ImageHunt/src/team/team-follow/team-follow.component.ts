import { Component, OnInit } from '@angular/core';
import { LiveService } from '../../shared/services/live.service';
import { ActivatedRoute } from '@angular/router';
import { Team } from '../../shared/team';
import { TeamPosition } from '../../shared/teamPosition';
import * as L from 'leaflet';

@Component({
    selector: 'team-follow',
    templateUrl: './team-follow.component.html',
    styleUrls: ['./team-follow.component.scss']
})
/** team-follow component*/
export class TeamFollowComponent implements OnInit {
    /** team-follow ctor */
  constructor(private _liveService: LiveService, private route: ActivatedRoute) {

    this._liveService._connection.on("PositionChanged",
      (team, dateOccured, position) => this.handleNewPosition(team, dateOccured, position));

    this._liveService._connection.start()
      .then(() => {
        console.log("Connection to SignalR successfull");
        this._liveService._connection.send("InitConnection");
      })
      .catch(error => console.error(error));
  }
  handleNewPosition(team: Team, dateOccured, position: L.LatLng) {
    this.positions.push({ team, dateOccured, position });
  }
  ngOnInit(): void {
    this.gameId = this.route.snapshot.params["gameId"];

  }
  positions: TeamPosition[] = [];
  gameId: number;
}
