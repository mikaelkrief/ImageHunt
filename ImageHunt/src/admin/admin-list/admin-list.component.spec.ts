/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AdminListComponent } from './admin-list.component';
import {Admin} from "../../shared/admin";
import { Observable } from "rxjs/Observable";
import {AdminService} from "../../shared/services/admin.service";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { SharedModule } from "../../shared/shared.module";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ConfirmationService } from "primeng/api";

import 'rxjs/add/observable/of';
import { Component } from "@angular/core";
import { HttpClient } from '@angular/common/http';
let component: AdminListComponent;
let fixture: ComponentFixture<AdminListComponent>;
class MockAdminService {
  getAllAdmins() {
    return new Observable<any>();
  }
}
@Component({
  selector: 'p-confirmDialog',
  template: ''
})
class FakeConfirmDialogComponent {
}
describe('admin-list component', () =>
{
  let adminsAsText = [{name: "kkskskk",
      email: "kkkskk@kjj.com",
      token: null,
      expirationTokenDate: null,
    games: [],
    role:0,
      id: 2
  },{name: "khkjjk",
      email: "kkkskk@kjj.com",
    token: null,
    expirationTokenDate: null,
    games: [],
    role:1,id: 3}];
  beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AdminListComponent],
            imports: [BrowserModule, RouterModule, FormsModule, SharedModule, ConfirmDialogModule],
            providers: [AdminService, ConfirmationService, HttpClient]
    });
    TestBed.overrideModule(ConfirmDialogModule,
      {
        set: {
          declarations: [FakeConfirmDialogComponent],
          exports: [FakeConfirmDialogComponent]
        }
      });
    TestBed.compileComponents();
        fixture = TestBed.createComponent(AdminListComponent);
        component = fixture.componentInstance;
    var adminService = fixture.debugElement.injector.get(AdminService);
    spyOn(adminService, 'getAllAdmins').and.callFake((params: any) => {
      console.log(`fake calling accept`);
      params.accept();
    });
  }));

  describe('getAdmins tests', () => {
    it('getAdmins test', async(() => {
      fixture = TestBed.createComponent(AdminListComponent);
      var adminService = fixture.debugElement.injector.get(AdminService);
      fixture.detectChanges();
      //var spy = spyOn(adminService, 'getAllAdmins').and.callFake((params: any) => {
      //  console.log(`fake calling accept`);
      //  params.accept();
      //});

      var admins = fixture.componentInstance.getAdmins();
      //expect(spy.calls.any()).toBe(true, 'getAdmins not called');
    }));
  });

  describe('deleteAdmin tests', () => {
    let confirmService = fixture.debugElement.injector.get(ConfirmationService);
    spyOn(confirmService, 'confirm').and.callFake((params: any) => {
      console.log(`fake calling accept`);
      params.accept();
    });
    it('deleteAdmin test', async(() => {
      fixture.componentInstance.deleteAdmin(1);
    }));
    
  });
});
