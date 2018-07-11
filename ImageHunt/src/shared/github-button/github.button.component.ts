import { Component, OnInit } from '@angular/core';
import {AdminService} from "../services/admin.service";
import {Admin} from "../admin";
import { AuthService } from "ng2-ui-auth";
import { LocalStorageService } from "angular-2-local-storage";

@Component({
  selector: 'github-button',
  templateUrl: './github.button.component.html',
  styleUrls: ['./github.button.component.scss']
})
/** github.button component*/
export class GithubButtonComponent implements OnInit {
  authenticated: boolean;
  userEmail: string;
  admin:Admin;
  /** github.button ctor */
  constructor(private auth: AuthService,
    private adminService: AdminService,
    private localStorageService: LocalStorageService) { 
  }

  /** Called by Angular after github.button component initialized */
  ngOnInit(): void {
    var expirationDate = <number>(this.localStorageService.get('expiration-date'));
    this.authenticated = new Date().getTime() < expirationDate;
    this.localStorageService.set('isAuthenticated', this.authenticated);
    if (this.authenticated)
      this.admin = <Admin>(this.localStorageService.get('connectedAdmin'));
    else
      this.admin = null;
  }

  authenticate() {
    this.auth.authenticate('github')
      .subscribe(
      {
        next: (response) => {
          let data = response.json();
          this.auth.setToken(data.access_token);
          var seconds = new Date().getSeconds() + data.expires_in;
          var expireDate = new Date().setSeconds(seconds);
          this.localStorageService.set('expiration-date', expireDate);

          this.userEmail = data.email;
        },
        complete: () => {
          this.authenticated = this.auth.isAuthenticated();
          this.localStorageService.set('isAuthenticated', this.authenticated);

          this.adminService.getAdminByEmail(this.userEmail)
            .subscribe(value => {
              this.admin = value.json();
              this.localStorageService.set('connectedAdmin', this.admin);
            });
        }

      });

  }
  logout() {
    this.auth.logout()
      .subscribe({
        complete: () => {
          this.authenticated = this.auth.isAuthenticated();
          this.localStorageService.remove('isAuthenticated');
          this.localStorageService.remove('connectedAdmin');
          this.localStorageService.remove('expiration-date');
          this.admin = null;
        }
      });
  }
}
