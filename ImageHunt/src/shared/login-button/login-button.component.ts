import { Component } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap';
import { LoginFormComponent } from '../../account/login-form/login-form.component';
import { LocalStorageService } from 'angular-2-local-storage';

@Component({
    selector: 'login-button',
    templateUrl: './login-button.component.html',
    styleUrls: ['./login-button.component.scss']
})
/** login-button component*/
export class LoginButtonComponent {
    /** login-button ctor */
  constructor(private _modalService: BsModalService, private localStorageService: LocalStorageService) {

  }
  login() {
    this.modalRef = this._modalService.show(LoginFormComponent, { ignoreBackdropClick: true });
  }
  logout() {
    this.localStorageService.remove('authToken');
  }
  get isAuthenticated(): boolean { return <boolean>(this.localStorageService.get('authToken')); }
  modalRef;
}
