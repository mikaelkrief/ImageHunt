/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { PlayerScoreboardComponent } from "./player-scoreboard.component";

let component: PlayerScoreboardComponent;
let fixture: ComponentFixture<PlayerScoreboardComponent>;

describe("player-scoreboard component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [PlayerScoreboardComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(PlayerScoreboardComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
