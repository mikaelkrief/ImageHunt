import { Component, OnInit } from '@angular/core';
import {GameService} from "../../shared/services/game.service";
import {Game} from "../../shared/game";
import { LocalStorageService } from "angular-2-local-storage";
import {Admin} from "../../shared/admin";
import { DatePipe } from '@angular/common';

@Component({
    selector: 'game-list',
    templateUrl: './game-list.component.html',
    styleUrls: ['./game-list.component.scss']
})
/** game-list component*/
export class GameListComponent implements OnInit
{
  games: Game[];
    /** game ctor */
    constructor(private gameService: GameService, private localStorageService: LocalStorageService) { }

    /** Called by Angular after game component initialized */
    ngOnInit(): void {
      var admin: Admin = <Admin>(this.localStorageService.get('connectedAdmin'));
      if (admin != null)
        this.gameService.getGameForAdmin(admin.id).subscribe(next=>this.games = next.json());
    }
}
