import { Component, OnInit } from '@angular/core';
import { UserService } from 'services/user.service';
import { AlertService } from 'services/alert.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { RegistrationFormComponent } from '../registration-form/registration-form.component';
import { ConfirmationService } from 'primeng/api';
import { GameAssignComponent } from "../game-assign/game-assign.component";
import { User } from 'shared/user';

@Component({
    selector: 'user-role',
    templateUrl: './user-role.component.html',
    styleUrls: ['./user-role.component.scss']
})
/** user-role component*/
export class UserRoleComponent implements OnInit {
    ngOnInit(): void {
      this._userService.getUsers()
        .subscribe(res => { this.users = res; });
    }
    /** user-role ctor */
  constructor(private _userService: UserService,
    private _alertService: AlertService,
    private _confirmationService: ConfirmationService,
    private _modalService: BsModalService) {

    }

  users: any[];
  saveUser(user) {
    this._userService.saveUser(user)
      .subscribe(res => { this._alertService.sendAlert(`L'utilisateur ${user.userName} a bien été sauvegardé`, "success", 3000); },
      error => {this._alertService.sendAlert(`Erreur lors de la sauvegarde de l'utilisateur ${user.userName}`, "danger", 10000);});
  }
  createUser() {
    this.modalRef = this._modalService.show(RegistrationFormComponent, { ignoreBackdropClick: true });
    this.modalRef.content.userCreated.subscribe(() => this._userService.getUsers()
      .subscribe(res => { this.users = res; }));
  }
  deleteUser(user) {
    this._confirmationService.confirm({
      message: "Etes-vous sur de vouloir supprimer cet utilisateur ?",
      accept: () =>
        this._userService.deleteUser(user)
          .subscribe(() => {
          this._alertService.sendAlert(`L'utilisateur ${user.userName} a bien été effacé`, "success", 3000);
          this._userService.getUsers()
            .subscribe(res => { this.users = res; });

          },
          error => {this._alertService.sendAlert("Erreur lors de l'effacement de l'utilisateur", "danger", 10000);})
    });
  }
  assignGame(user: User) {
    this.modalRef = this._modalService.show(GameAssignComponent, { ignoreBackdropClick: true, initialState: { user } });
    this.modalRef.content.user = user;
    this._modalService.onHidden.subscribe(() =>
      this._userService.getUsers()
      .subscribe(res => { this.users = res; }));
  }

  modalRef;
}
