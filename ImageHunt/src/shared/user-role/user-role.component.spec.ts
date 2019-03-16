/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { UserRoleComponent } from "./user-role.component";

let component: UserRoleComponent;
let fixture: ComponentFixture<UserRoleComponent>;

describe("user-role component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [UserRoleComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(UserRoleComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
