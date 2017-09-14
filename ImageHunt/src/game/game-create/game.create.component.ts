import { Component, OnInit } from '@angular/core';
import {Globals} from "../../shared/globals";
import {Admin} from "../../shared/admin";

@Component({
    selector: 'game-create',
    templateUrl: './game.create.component.html',
    styleUrls: ['./game.create.component.scss']
})
/** game.create component*/
export class GameCreateComponent implements OnInit
{
  admin:Admin;
    /** game.create ctor */
    constructor(private globals:Globals) { }

    /** Called by Angular after game.create component initialized */
    ngOnInit(): void {
      this.admin = this.globals.connectedUser;
    }
}
