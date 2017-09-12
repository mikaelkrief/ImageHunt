import { Component, OnInit } from '@angular/core';
import { AuthService } from "ng2-ui-auth";

@Component({
    selector: 'google-button',
    templateUrl: './google.button.component.html',
    styleUrls: ['./google.button.component.scss']
})
/** google.button component*/
export class GoogleButtonComponent implements OnInit
{
    authenticated: boolean;
    /** google.button ctor */
    constructor(private auth: AuthService) { }

    /** Called by Angular after google.button component initialized */
    ngOnInit(): void { }

    authenticate() {
      this.auth.authenticate('google')
        .subscribe({ complete: () => this.authenticated = !this.authenticated});
      
    }
}
