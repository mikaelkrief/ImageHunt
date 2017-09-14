import { Component, OnInit } from '@angular/core';
import { AuthService } from "ng2-ui-auth";
import {AdminService} from "../services/admin.service";
import {Globals} from "../globals";

@Component({
  selector: 'google-button',
  templateUrl: './google.button.component.html',
  styleUrls: ['./google.button.component.scss']
})
/** google.button component*/
export class GoogleButtonComponent implements OnInit {
  authenticated: boolean;
  userEmail: string;
  /** google.button ctor */
  constructor(private auth: AuthService, private adminService: AdminService, private globals:Globals) { }

  /** Called by Angular after google.button component initialized */
  ngOnInit(): void {
    this.authenticated = this.auth.isAuthenticated();
  }

  authenticate() {
    this.auth.authenticate('google')
      .subscribe(
      {
        next: (response) => {
          let data = response.json();
          this.auth.setToken(data.access_token);
          this.userEmail = data.email;
          console.log(this.userEmail);
        },
        complete: () => {
          this.authenticated = this.authenticated = this.auth.isAuthenticated();
          this.adminService.getAdminByEmail(this.userEmail).subscribe(value => this.globals.connectedUser = value.json());
        }

      });

  }
  logout() {
    this.auth.logout()
      .subscribe({complete:()=>this.authenticated = this.auth.isAuthenticated()});
  }
}
