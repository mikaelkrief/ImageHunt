/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { AdminCreateComponent } from "./admin-create.component";

let component: AdminCreateComponent;
let fixture: ComponentFixture<AdminCreateComponent>;

describe("admin-create component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [AdminCreateComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(AdminCreateComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
