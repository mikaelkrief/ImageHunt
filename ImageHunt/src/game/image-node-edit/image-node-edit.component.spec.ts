/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { ImageNodeEditComponent } from "./image-node-edit.component";

let component: ImageNodeEditComponent;
let fixture: ComponentFixture<ImageNodeEditComponent>;

describe("imageNode-edit component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [ImageNodeEditComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(ImageNodeEditComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
