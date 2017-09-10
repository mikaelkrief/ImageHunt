import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-game',
    templateUrl: './game.component.html',
    styleUrls: ['./game.component.scss']
})
/** game component*/
export class GameComponent implements OnInit
{
    /** game ctor */
    constructor() { }

    /** Called by Angular after game component initialized */
    ngOnInit(): void { }
}