import { Component, AfterViewInit, OnInit, NgZone } from '@angular/core';
import { environment } from '../../environments/environment';
declare const gapi: any;

@Component({
  selector: 'google-signin',
  templateUrl: './google-signin.component.html',
  styleUrls: ['./google-signin.component.scss']
})
/** google-signin component*/
export class GoogleSigninComponent implements OnInit, AfterViewInit  {
    ngAfterViewInit(): void {
      //this.googleInit();
    }
  ngOnInit() {
    //this.authService.authState.subscribe((user) => {
    //  this.user = user;
    //  this.loggedIn = (user != null);
    //});
  }
  private scope = [
    'profile',
    'email',
    'https://www.googleapis.com/auth/plus.me',
    'https://www.googleapis.com/auth/contacts.readonly',
    'https://www.googleapis.com/auth/admin.directory.user.readonly'
  ].join(' ');
  constructor(ngZone: NgZone) {
    window['onSignIn'] = (user) => ngZone.run(() => this.onSignIn(user));
  
  }
  public auth2: any;


  //googleInit() {
  //  gapi.load('auth2', () => {
  //    this.auth2 = gapi.auth2.init({
  //      client_id: environment.GOOGLE_CLIENT_ID,
  //      cookiepolicy: 'single_host_origin',
  //      scope: this.scope
  //    });
  //    this.attachSignin(document.getElementById("googleBtn"));
  //  });

  //}
  onSignIn(googleUser) {
    let profile = googleUser.getBasicProfile();
    console.log('Token || ' + googleUser.getAuthResponse().id_token);
    console.log('ID: ' + profile.getId());
  }
  public attachSignin(element) {
    this.auth2.attachClickHandler(element, {},
      (googleUser) => {
        let profile = googleUser.getBasicProfile();
        console.log('Token || ' + googleUser.getAuthResponse().id_token);
        console.log('ID: ' + profile.getId());
        // ...
      }, function (error) {
        console.log(JSON.stringify(error, undefined, 2));
      });
  }
}
