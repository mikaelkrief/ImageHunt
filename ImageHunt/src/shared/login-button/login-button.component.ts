import { Component } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap';
import { LoginFormComponent } from '../../account/login-form/login-form.component';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
    selector: 'login-button',
    templateUrl: './login-button.component.html',
    styleUrls: ['./login-button.component.scss']
})
/** login-button component*/
export class LoginButtonComponent {
    /** login-button ctor */
  constructor(private _modalService: BsModalService,
    private _jwtHelperService: JwtHelperService) {

  }
  login() {
    this.modalRef = this._modalService.show(LoginFormComponent, { ignoreBackdropClick: true });
  }
  logout() {
    localStorage.removeItem('authToken');
  }
  get isAuthenticated(): boolean { return <boolean>(!!localStorage.getItem('authToken')); }
  get userName(): string {
    const rawToken = localStorage.getItem('authToken');
    const decodedToken = this._jwtHelperService.decodeToken(rawToken);
    return decodedToken.sub;
  }
  modalRef;
}
