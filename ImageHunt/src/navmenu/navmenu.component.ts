import { Component, OnInit } from '@angular/core';
import {LocalStorageService} from "angular-2-local-storage";
import { BsModalService } from 'ngx-bootstrap';
import { LoginFormComponent } from '../account/login-form/login-form.component';

@Component({
    selector: 'navmenu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.scss']
})
/** navmenu component*/
export class NavmenuComponent implements OnInit
{
  constructor(private localStorageService: LocalStorageService) { }


  get isAuthenticated(): boolean { return <boolean>(this.localStorageService.get('authToken')); }

  isCollapsed: boolean;
    /** navmenu ctor */
    /** Called by Angular after navmenu component initialized */
    ngOnInit(): void {
  }

  modalRef;
}
