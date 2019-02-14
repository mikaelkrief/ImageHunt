import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/services/user.service';
import { LocalStorageService } from 'angular-2-local-storage';
import { NgForm } from '@angular/forms';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {

  constructor(private _userService: UserService,
    private _localStorageService: LocalStorageService,
    private _alertService: AlertService) {
  }

  ngOnInit() {
  }

  login(form: NgForm) {

    this._userService.login(form.form.value.userName, form.form.value.password)
      .subscribe(res => {
          this._localStorageService.set("authToken", res.value);
          this._alertService.sendAlert("Connexion réussie", "success", 3000);
        },
        error => {
          this._alertService.sendAlert("Impossible de vous connecter, vérifier votre login/mot de passe",
            "danger",
            10000);
        });
  }
}
