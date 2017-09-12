import { Injectable } from '@angular/core';
import { Http } from "@angular/http";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';

@Injectable()
export class TeamService {
  constructor(private http: Http) { }
  getTeams() {
    return this.http.get('api/team')
        .do(t=>console.log(t))
        .map(t=>t.json());

  }
}
