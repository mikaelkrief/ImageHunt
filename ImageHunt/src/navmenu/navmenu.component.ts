import { JwtHelperService } from "@auth0/angular-jwt";

@Component({
  selector: "navmenu",
  templateUrl: "./navmenu.component.html",
  styleUrls: ["./navmenu.component.scss"]
})
/** navmenu component*/
export class NavmenuComponent implements OnInit {
  constructor(private _jwtHelperService: JwtHelperService) {
  }


  get isAuthenticated(): boolean { return (!!localStorage.getItem("authToken")) as boolean; }

  isCollapsed: boolean;

  /** navmenu ctor */
  /** Called by Angular after navmenu component initialized */
  ngOnInit(): void {
  }

  isDisplayed(authorizedRoles: string[]) {
    if (authorizedRoles.includes("*"))
      return true;
    const token = localStorage.getItem("authToken");
    const role =
      this._jwtHelperService.decodeToken(token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return (authorizedRoles.includes(role));
  }

  modalRef;
}
