import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import "rxjs/add/operator/map";
import "rxjs/add/operator/toPromise";
import {Admin} from "../admin";

@Injectable()
export class AdminService {
  constructor(private http: Http) { }
  getAllAdmins() {
    return this.http.get('api/Admin/GetAllAdmins')
      .map(a => a.json());
  }
  createAdmin(newAdmin: Admin) {
    return this.http.post('api/Admin/', newAdmin);
  }
  deleteAdmin(adminId: number) {
    return this.http.delete('api/Admin/' + adminId);
  }
  getAdminByEmail(email: string) {
    return this.http.get('api/Admin/ByEmail/' + email);
  }
}
