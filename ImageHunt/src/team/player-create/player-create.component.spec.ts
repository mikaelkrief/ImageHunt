/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { PlayerCreateComponent } from "./player-create.component";

let component: PlayerCreateComponent;
let fixture: ComponentFixture<PlayerCreateComponent>;

describe("player-create component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [PlayerCreateComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(PlayerCreateComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
