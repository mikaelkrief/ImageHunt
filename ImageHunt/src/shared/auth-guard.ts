import { JwtHelperService } from "@auth0/angular-jwt";


@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  constructor(private _jwtHelperService: JwtHelperService) {

  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const authToken = localStorage.getItem("authToken");
    if (!authToken)
      return false;
    const userRole =
      this._jwtHelperService.decodeToken(authToken)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    const requiredRoles: string[] = next.data["roles"];
    return requiredRoles.indexOf(userRole) !== -1;
  }

}
