/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { NodeListComponent } from "./node.list.component";

let component: NodeListComponent;
let fixture: ComponentFixture<NodeListComponent>;

describe("node-list component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [NodeListComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(NodeListComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
