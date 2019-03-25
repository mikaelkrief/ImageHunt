import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { UserService } from 'services/user.service';
import { AlertService } from 'services/alert.service';
import { ConfirmationService } from 'primeng/api';

@Component({
    selector: 'edit-user',
    templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.scss']
})
/** edit-user component*/
export class EditUserComponent implements OnInit {
  ngOnInit(): void {
    this.userService.getUser(this.userName)
      .subscribe(res => {this.user = res});
  }
    /** edit-user ctor */
  constructor(private _route: ActivatedRoute,
    private userService: UserService,
    private alertService: AlertService,
  private confirmationService: ConfirmationService) {
    this.userName = this._route.snapshot.params['username'];
  }
  save(form: NgForm) {
    const user = {
      id: this.user.id,
      userName: this.user.userName,
      telegram: form.form.value.tgUsername,
      currentPassword: form.form.value.currentpassword,
      newPassword: form.form.value.newpassword
    }
    this.userService.saveUser(user)
      .subscribe(res => { this.alertService.sendAlert(`The user ${this.user.userName} had been correctly modified`, "success", 3000); },
      error => {
        this.alertService.sendAlert(`Unable to edit ${this.user.userName} for the following reason: `, "danger", 10000);
        error.error.map(err => this.alertService.sendAlert(`${err.description}`, "danger", 10000));

      });
  }
  deleteUser() {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete user ${this.userName}?`,
      accept: () => this.userService.deleteUser(this.user)
        .subscribe(res => this.alertService.sendAlert(`User ${this.userName} had been deleted`, "success", 5000),
          error => {
            this.alertService.sendAlert(`Unable to delete ${this.user.userName} for the following reason: `,
              "danger",
              10000);
            error.error.map(err => this.alertService.sendAlert(`${err.description}`, "danger", 10000));

          })
    });
  }

  userName: string;
  user;
}
