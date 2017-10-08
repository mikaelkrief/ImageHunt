/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AdminListComponent } from './admin-list.component';
import {Admin} from "../../shared/admin";
import { Observable } from "rxjs/Observable";
import {AdminService} from "../../shared/services/admin.service";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
let component: AdminListComponent;
let fixture: ComponentFixture<AdminListComponent>;
class MockAdminService {
  getAllAdmins() {
    return new Observable<any>();
  }
}
describe('admin-list component', () =>
{
  let adminServiceStub = {
    getAllAdmins: new Observable<any>()
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

  it('getAdmins test', async(() => {
    fixture.detectChanges();
      fixture.nativeElement.getAdmins();
    }));
  it('deleteAdmin test', async(() => {
  }));
});
