import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/services/user.service';
import { LocalStorageService } from 'angular-2-local-storage';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})
export class RegistrationFormComponent implements OnInit {

  constructor(private userService: UserService, private localStorageService: LocalStorageService, private _alertService: AlertService) { }

  ngOnInit() {
  }
  register(form) {
    let user = form.form.value;
    this.userService.register(user.email, user.password, user.userName, user.telegram)
      .subscribe(res => {
        this.localStorageService.set("authToken", res);
        this._alertService.sendAlert(`L'utilisateur ${user.userName} a bien été créé`, "success", 3000);
        },
      error => {
        this.localStorageService.remove("authToken");
        if (error.error)
          error.error.errors.map(e => this._alertService.sendAlert(e.description, "danger", 10000));
      });
  }
}
