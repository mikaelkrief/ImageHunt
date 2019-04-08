import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap';
import { LoginFormComponent } from '../account/login-form/login-form.component';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
    selector: 'navmenu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.scss']
})
/** navmenu component*/
export class NavmenuComponent implements OnInit
{
  constructor(private _jwtHelperService: JwtHelperService) {
  }


  get isAuthenticated(): boolean { return <boolean>(!!localStorage.getItem('authToken')); }

  isCollapsed: boolean;
    /** navmenu ctor */
    /** Called by Angular after navmenu component initialized */
    ngOnInit(): void {
  }
  isDisplayed(authorizedRoles: string[]) {
    if (authorizedRoles.includes("*"))
      return true;
    var token = localStorage.getItem("authToken");
    var role = this._jwtHelperService.decodeToken(token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return (authorizedRoles.includes(role));
  }
  modalRef;
}
