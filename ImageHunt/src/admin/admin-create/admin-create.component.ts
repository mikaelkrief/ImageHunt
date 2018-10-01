import { Component, Output, EventEmitter } from '@angular/core';
import { Admin } from '../../shared/admin';
import { NgForm, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    selector: 'admin-create',
    templateUrl: './admin-create.component.html',
    styleUrls: ['./admin-create.component.scss']
})
/** admin-create component*/
export class AdminCreateComponent {
  @Output() adminCreated = new EventEmitter<Admin>();
  adminCreateForm: FormGroup;

    /** admin-create ctor */
  constructor(public bsModalRef: BsModalRef,) {

  }
  name: string;
  email: string;
  role: any;
  createAdmin(form: NgForm) {
    let admin: Admin = { id: 0, name: this.name, email: this.email, games: null, role: this.role};
    this.adminCreated.emit(admin);
    this.adminCreateForm.reset();
  }
}
