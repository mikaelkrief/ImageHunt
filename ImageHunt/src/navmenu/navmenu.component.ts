import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'navmenu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.scss']
})
/** navmenu component*/
export class NavmenuComponent implements OnInit
{
    /** navmenu ctor */
    constructor() { }

    /** Called by Angular after navmenu component initialized */
    ngOnInit(): void { }
}