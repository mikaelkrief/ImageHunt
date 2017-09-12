import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import {Admin} from "../../shared/admin";
import {Game} from "../../shared/game";
import {AdminService} from "../../shared/services/admin.service";


@Component({
    selector: 'new-admin',
    templateUrl: './new.admin.component.html',
    styleUrls: ['./new.admin.component.scss']
})
/** newAdmin component*/
export class NewAdminComponent implements OnInit
{
    /** newAdmin ctor */
    constructor(private _adminService: AdminService) { }

    /** Called by Angular after newAdmin component initialized */
    ngOnInit(): void { }

    createAdmin(form: NgForm) {
      var newAdmin: Admin = {id:0, email: form.value.email, name: form.value.name, games: null };
      this._adminService.createAdmin(newAdmin);
    }
}
