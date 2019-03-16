import { Injectable } from '@angular/core';
import {
  CanActivate, Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild,
  NavigationExtras,
  CanLoad, Route
} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';


@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private _jwtHelperService: JwtHelperService) {

  }
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const authToken = localStorage.getItem("authToken");
    if (!authToken)
      return false;
    var userRole =
      this._jwtHelperService.decodeToken(authToken)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    var requiredRoles: string[] = next.data["roles"];
    return requiredRoles.indexOf(userRole) !== -1;
  }

}
