import { Component, OnInit } from '@angular/core';
import {AuthService} from "ng2-ui-auth";

@Component({
    selector: 'navmenu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.scss']
})
/** navmenu component*/
export class NavmenuComponent implements OnInit
{
  isAuthenticated:boolean;
    /** navmenu ctor */
  constructor(private auth: AuthService) { }

    /** Called by Angular after navmenu component initialized */
    ngOnInit(): void {
      this.isAuthenticated = this.auth.isAuthenticated();
    }
}
