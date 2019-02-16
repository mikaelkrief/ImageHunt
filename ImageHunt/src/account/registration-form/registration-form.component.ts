import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UserService } from '../../shared/services/user.service';
import { LocalStorageService } from 'angular-2-local-storage';
import { AlertService } from '../../shared/services/alert.service';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})
export class RegistrationFormComponent implements OnInit {
  @Output() userCreated = new EventEmitter();

  constructor(private userService: UserService,
    private localStorageService: LocalStorageService,
    private _alertService: AlertService,
    public bsModalRef: BsModalRef) { }

  ngOnInit() {
  }
  register(form) {
    let user = form.form.value;
    this.userService.register(user.email, user.password, user.userName, user.telegram)
      .subscribe(res => {
        this._alertService.sendAlert(`L'utilisateur ${user.userName} a bien été créé`, "success", 3000);
          this.userCreated.emit();
          this.bsModalRef.hide();
        },
      error => {
        this.localStorageService.remove("authToken");
        if (error.error)
          error.error.errors.map(e => this._alertService.sendAlert(e.description, "danger", 10000));
      });
  }
}
