/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { DualListComponent } from "./dual-list.component";

let component: DualListComponent;
let fixture: ComponentFixture<DualListComponent>;

describe("dual-list component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [DualListComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(DualListComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
