import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import "rxjs/add/operator/map";
import "rxjs/add/operator/toPromise";
import {Admin} from "../admin";
import { Observable } from "rxjs";

@Injectable()
export class AdminService {
  constructor(private http: HttpClient) { }
  getAllAdmins() {
    return this.http.get('api/Admin/GetAllAdmins');
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
  assignGame(adminId: number, gameId: number, assign: boolean) {
    return this.http.put(`api/Admin/Assign/${adminId}/${gameId}?assign=${assign}`, null);
  }
}
