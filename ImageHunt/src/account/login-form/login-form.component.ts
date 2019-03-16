import { UserService } from "../../shared/services/user.service";
import { AlertService } from "../../shared/services/alert.service";

@Component({
  selector: "app-login-form",
  templateUrl: "./login-form.component.html",
  styleUrls: ["./login-form.component.css"]
})
export class LoginFormComponent implements OnInit {

  constructor(private _userService: UserService,
    private _alertService: AlertService,
    public bsModalRef: BsModalRef) {
  }

  ngOnInit() {
  }

  login(form: NgForm) {

    this._userService.login(form.form.value.userName, form.form.value.password)
      .subscribe(res => {
          localStorage.setItem("authToken", res.value);
          this._alertService.sendAlert("Connexion réussie", "success", 3000);
          this.bsModalRef.hide();
        },
        error => {
          this._alertService.sendAlert("Impossible de vous connecter, vérifier votre login/mot de passe",
            "danger",
            10000);
        });
  }
}
