import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'team',
    templateUrl: './team.component.html',
    styleUrls: ['./team.component.scss']
})
/** team component*/
export class TeamComponent implements OnInit
{
    /** team ctor */
    constructor() { }

    /** Called by Angular after team component initialized */
    ngOnInit(): void { }
}