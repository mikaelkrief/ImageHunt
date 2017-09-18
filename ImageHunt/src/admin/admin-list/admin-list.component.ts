import { Component, OnInit } from '@angular/core';
import {AdminService} from "../../shared/services/admin.service";
import {Admin} from "../../shared/admin";
import { NgForm } from "@angular/forms";

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
    this.getAdmins();
  }
  getAdmins() {
    this._adminService.getAllAdmins()
      .subscribe(res => this.admins = res,
        err => console.error(err.status));
  }
  deleteAdmin(adminId: number) {
    this._adminService.deleteAdmin(adminId)
      .subscribe(null, null, () => this.getAdmins());
  }
  createAdmin(form: NgForm) {
    var admin: Admin = { id: 0, name: form.value.name, email: form.value.email, games: null };
    this._adminService.createAdmin(admin)
      .subscribe(null, null, () => {
        this.getAdmins();
        form.resetForm();
      });
  }
}
