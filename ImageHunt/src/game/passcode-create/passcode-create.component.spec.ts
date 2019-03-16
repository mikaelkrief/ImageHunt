/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { PasscodeCreateComponent } from "./passcode-create.component";

let component: PasscodeCreateComponent;
let fixture: ComponentFixture<PasscodeCreateComponent>;

describe("passcode-create component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [PasscodeCreateComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(PasscodeCreateComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
