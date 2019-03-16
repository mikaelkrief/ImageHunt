﻿/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { NodeCreateComponent } from "./node.create.component";

let component: NodeCreateComponent;
let fixture: ComponentFixture<NodeCreateComponent>;

describe("node-create component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [NodeCreateComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(NodeCreateComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
