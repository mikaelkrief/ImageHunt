import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-team',
    templateUrl: './team.component.html',
    styleUrls: ['./team.component.scss']
})
/** team component*/
export class TeamComponent implements OnInit
{
    teams = 
    [
      { name: "Team1", members: [{ pseudo: "Player1" }, { pseudo: "Player2" }, { pseudo: "Player3" }] },
      { name: "Team2", members: [{ pseudo: "Player4" }, { pseudo: "Player5" }, { pseudo: "Player6" }, { pseudo: "Player7" }] }
    ];
    /** team ctor */
    constructor() { }

    /** Called by Angular after team component initialized */
    ngOnInit(): void { }
}
