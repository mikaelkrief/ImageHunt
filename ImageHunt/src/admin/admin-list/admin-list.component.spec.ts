/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AdminListComponent } from './admin-list.component';
import {Admin} from "../../shared/admin";
import { Observable } from "rxjs/Observable";
import {AdminService} from "../../shared/services/admin.service";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import 'rxjs/add/observable/of';
let component: AdminListComponent;
let fixture: ComponentFixture<AdminListComponent>;
class MockAdminService {
  getAllAdmins() {
    return new Observable<any>();
  }
}
describe('admin-list component', () =>
{
  let adminsAsText = '[{\"name\": \"kkskskk\",\"email\": \"kkkskk@kjj.com\",\"token\": null,\"expirationTokenDate\": null,\"games\": [],\"id\": 2},{\"name\": \"khkjjk\",\"email\": \"kkkskk@kjj.com\",\"token\": null,\"expirationTokenDate\": null,\"games\": [],\"id\": 3}]'; 
    let adminServiceStub = {
    getAllAdmins: function() {
      return Observable.of(adminsAsText);
    }
  }
  beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AdminListComponent],
            imports: [BrowserModule, RouterModule, FormsModule],
            providers: [{provide: AdminService, useValue: adminServiceStub}]
        });
        fixture = TestBed.createComponent(AdminListComponent);
        component = fixture.componentInstance;
  }));

  describe('getAdmins tests', () => {
    it('getAdmins test', async(() => {
      var adminService = fixture.debugElement.injector.get(AdminService);
      fixture.detectChanges();
      var spy = spyOn(adminService, 'getAllAdmins');
      var admins = fixture.componentInstance.getAdmins();
      expect(spy.calls.any()).toBe(true, 'getAdmins not called');
    }));
    
  });
  describe('deleteAdmin tests', () => {
    it('deleteAdmin test', async(() => {
      fixture.componentInstance.deleteAdmin(1);
    }));
    
  });
});
