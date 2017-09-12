import { Component, OnInit } from '@angular/core';
import {AdminService} from "../../shared/services/admin.service";
import {Admin} from "../../shared/admin";

@Component({
  selector: 'admin-list',
  templateUrl: './admin-list.component.html',
  styleUrls: ['./admin-list.component.scss']
})
/** admin component*/
export class AdminListComponent implements OnInit {
  admins: Admin[];
  /** admin ctor */
  constructor(private _adminService: AdminService) { }

  /** Called by Angular after admin component initialized */
  ngOnInit(): void {
    this._adminService.getAllAdmins()
      .subscribe(res => this.admins = res,
      err => console.error(err.status));
  }
  deleteAdmin(adminId: number) {
    this._adminService.deleteAdmin(adminId)
      .then(res => { this.ngOnInit(); });
  }
}
