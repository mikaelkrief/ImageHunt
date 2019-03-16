/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { MapThumbnailComponent } from "./map.thumbnail.component";

let component: MapThumbnailComponent;
let fixture: ComponentFixture<MapThumbnailComponent>;

describe("map-thumbnail component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [MapThumbnailComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(MapThumbnailComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
