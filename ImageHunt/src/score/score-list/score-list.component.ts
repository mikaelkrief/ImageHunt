import { Component, OnInit } from '@angular/core';
import { GameService } from "../../shared/services/game.service";
import { Game } from '../../shared/game';
import { GameAction } from '../../shared/gameAction';
import { Score } from '../../shared/score';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-score-list',
  templateUrl: './score-list.component.html',
  styleUrls: ['./score-list.component.css']
})
export class ScoreListComponent implements OnInit {
  games: Game[];
  scores: Score[];
  selectedGame: Game;

  columnDefs = [
    { headerName: 'Equipe', field: 'team.name' },
    { headerName: 'EquipeId', field: 'team.id' },
    { headerName: 'Score', field: 'pointsEarned' },
    ];
  constructor(private _gameService: GameService, private _route: ActivatedRoute) { }

  ngOnInit() {
    let gameId = this._route.snapshot.params["gameId"];

    this._gameService.getGameById(gameId).subscribe((game: Game) => {
      this.game = game;
    });
    this._gameService.getScoreForGame(gameId)
      .subscribe((scores: Score[]) => {
        this.scores = scores;
        this.scores = this.scores.sort((a, b) => b.points - a.points);
      });

  }
  colorFromRank(rank: number): string {
    switch (rank) {
    case 0:
        return 'gold';
      case 1:
        return 'silver';
      case 2:
        return 'tan';
      default:
        return 'black';
    }
  }

  game: Game;
}
