import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import "rxjs/add/operator/map";
import "rxjs/add/operator/toPromise";
import {Admin} from "../admin";
import { JwtHttp } from "ng2-ui-auth";

@Injectable()
export class AdminService {
  constructor(private http: JwtHttp) { }
  getAllAdmins() {
    return this.http.get('api/admin')
      .map(a => a.json());
  }
  createAdmin(newAdmin: Admin) {
    return this.http.post('api/admin/', newAdmin).toPromise();
  }
  deleteAdmin(adminId: number) {
    return this.http.delete('api/admin/' + adminId).toPromise();
  }
  getAdminByEmail(email: string) {
    return this.http.get('api/admin/ByEmail/' + email);
  }
}
