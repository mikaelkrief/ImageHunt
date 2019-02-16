import { Injectable } from '@angular/core';
import { Response,  RequestOptions } from '@angular/http';
import { HttpClient, HttpHeaders } from "@angular/common/http";


import { BaseService } from "./base.service";

import { Observable, BehaviorSubject } from 'rxjs/Rx';
import { UserRegistration } from '../userRegistration';

// Add the RxJS Observable operators we need in this app.


@Injectable()

export class UserService extends BaseService {
  getUsers() { return this.http.get<any[]>(this.baseUrl + "Account"); }

  baseUrl: string = '';

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private loggedIn = false;

  constructor(private http: HttpClient) {
    super();
    this.loggedIn = !!localStorage.getItem('authToken');
    // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    // header component resulting in authed user nav links disappearing despite the fact user is still logged in
    this._authNavStatusSource.next(this.loggedIn);
    this.baseUrl = 'api/';
  }

  register(email: string, password: string, username: string, telegram: string) {
    var identity = { email: email, password: password, login: username, telegram: telegram };

    return this.http.post(this.baseUrl + "Account/Register", identity);
  }

  login(userName: string, password: string):Observable<any> {
    return this.http.post(this.baseUrl + 'Account/Login', { userName, password });
  }


  saveUser(user) { return this.http.put(this.baseUrl + 'Account/', user); }

  deleteUser(user) { return this.http.delete(this.baseUrl + "Account/" + user.id); }
}
