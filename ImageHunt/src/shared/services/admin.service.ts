import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import "rxjs/add/operator/map";
import "rxjs/add/operator/toPromise";
import {Admin} from "../admin";
import { Observable } from "rxjs";

@Injectable()
export class AdminService {
  constructor(private http: HttpClient) {}
  getAllAdmins() {
    return this.http.get('api/Admin/GetAllAdmins', {headers: this.headers});
  }
  createAdmin(newAdmin: Admin) {
    return this.http.post('api/Admin/', newAdmin, { headers: this.headers });
  }
  deleteAdmin(adminId: number) {
    return this.http.delete('api/Admin/' + adminId, { headers: this.headers });
  }
  getAdminByEmail(email: string) {
    return this.http.get('api/Admin/ByEmail/' + email, { headers: this.headers });
  }
  assignGame(adminId: number, gameId: number, assign: boolean) {
    return this.http.put(`api/Admin/Assign/${adminId}/${gameId}?assign=${assign}`, null, { headers: this.headers });
  }

  headers: HttpHeaders;
}
