/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { BatchNodeComponent } from "./batch-node.component";

let component: BatchNodeComponent;
let fixture: ComponentFixture<BatchNodeComponent>;

describe("batch-node component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [BatchNodeComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(BatchNodeComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
