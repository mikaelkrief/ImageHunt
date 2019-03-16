/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { MapDetail3Component } from "./map-detail3.component";

let component: MapDetail3Component;
let fixture: ComponentFixture<MapDetail3Component>;

describe("map-detail3 component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [MapDetail3Component],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(MapDetail3Component);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
