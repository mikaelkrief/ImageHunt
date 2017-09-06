import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import {Team }  from './Team';
import { TEAMS } from "./mock-team";
import 'rxjs/add/operator/toPromise';

@Injectable()
export class TeamService {
  constructor(private http: Http) {  }
  getTeams():Promise<Team[]> {
    let promise = new Promise((resolve, reject) => {
      this.http.get('api/team')
        .toPromise();
    });
    return promise;
  }
}
