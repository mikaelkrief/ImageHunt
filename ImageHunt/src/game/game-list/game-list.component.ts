import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'game-list',
    templateUrl: './game-list.component.html',
    styleUrls: ['./game-list.component.scss']
})
/** game-list component*/
export class GameListComponent implements OnInit
{
    /** game ctor */
    constructor() { }

    /** Called by Angular after game component initialized */
    ngOnInit(): void { }
}
