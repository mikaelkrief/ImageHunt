/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GithubButtonComponent } from './github.button.component';
import { AuthService } from "ng2-ui-auth";
import {AdminService} from "../services/admin.service";
import { LocalStorageService } from "angular-2-local-storage";
let component: GithubButtonComponent;
let fixture: ComponentFixture<GithubButtonComponent>;

describe('github.button component', () =>
{
    beforeEach(async(() => {
      var authServiceStub = {
      };
      var adminServiceStub = {
      };
      var localStorageServiceStub = {
        getData: [{ key: 'expiration-date', value: new Date() }],
        get: function(key) {
          return this.getData.find(d => d.key == key).value;
        }
      }
        TestBed.configureTestingModule({
          declarations: [GithubButtonComponent],
            imports: [ BrowserModule ],
            providers: [
              { provide: ComponentFixtureAutoDetect, useValue: true },
              { provide: AuthService, useValue: authServiceStub },
              { provide: AdminService, useValue: adminServiceStub },
              { provide: LocalStorageService, useValue: localStorageServiceStub}
            ]
      });
    }));
  fixture = TestBed.createComponent(GithubButtonComponent);
      component = fixture.componentInstance;

    it('Check Authentication at startup', (() => {
      fixture.detectChanges();
      //var localStorageServiceStub = TestBed.get(LocalStorageService);
      expect(component.authenticated).toEqual(true);
    }));
});
