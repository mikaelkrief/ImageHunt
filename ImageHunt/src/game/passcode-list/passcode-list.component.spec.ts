﻿/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { PasscodeListComponent } from "./passcode-list.component";

let component: PasscodeListComponent;
let fixture: ComponentFixture<PasscodeListComponent>;

describe("passcode-list component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [PasscodeListComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(PasscodeListComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
