/// <reference path="../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { PageNotFoundComponent } from "./page.not.found.component";

let component: PageNotFoundComponent;
let fixture: ComponentFixture<PageNotFoundComponent>;

describe("pageNotFound component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [PageNotFoundComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(PageNotFoundComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
      }));
  });
