import { Component, OnInit } from '@angular/core';
import { GameService } from "../../shared/services/game.service";
import { Game } from '../../shared/game';
import { GameAction } from '../../shared/gameAction';
import { Score } from '../../shared/score';

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
  constructor(private _gameService: GameService) { }

  ngOnInit() {
    this._gameService.getGameReviewed().subscribe(next => {
      this.games = next.json();
    });
  }
  onGameChange(game) {
    this._gameService.getScoreForGame(game.id)
      .subscribe(list => {
        this.scores = list.json();
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

}
