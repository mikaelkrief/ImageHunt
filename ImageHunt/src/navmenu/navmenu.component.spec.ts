﻿/// <reference path="../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { NavmenuComponent } from "./navmenu.component";

let component: NavmenuComponent;
let fixture: ComponentFixture<NavmenuComponent>;

describe("navmenu component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [NavmenuComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(NavmenuComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
