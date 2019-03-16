/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { GameActionDetailComponent } from "./game-action-detail.component";

let component: GameActionDetailComponent;
let fixture: ComponentFixture<GameActionDetailComponent>;

describe("game-action-detail component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [GameActionDetailComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(GameActionDetailComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
