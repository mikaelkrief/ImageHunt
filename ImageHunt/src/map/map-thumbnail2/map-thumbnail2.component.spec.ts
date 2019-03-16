/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { MapThumbnail2Component } from "./map-thumbnail2.component";

let component: MapThumbnail2Component;
let fixture: ComponentFixture<MapThumbnail2Component>;

describe("map-thumbnail2 component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [MapThumbnail2Component],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(MapThumbnail2Component);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
