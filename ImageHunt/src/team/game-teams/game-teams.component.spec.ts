/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { GameTeamsComponent } from "./game-teams.component";

let component: GameTeamsComponent;
let fixture: ComponentFixture<GameTeamsComponent>;

describe("game-teams component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [GameTeamsComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(GameTeamsComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
