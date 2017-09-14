import { Component, OnInit } from '@angular/core';
import {Globals} from "../shared/globals";

@Component({
  moduleId: module.id,
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
/** home component*/
export class HomeComponent implements OnInit
{
    /** home ctor */
    constructor(private globals:Globals) { }

    /** Called by Angular after home component initialized */
    ngOnInit(): void { }
}
