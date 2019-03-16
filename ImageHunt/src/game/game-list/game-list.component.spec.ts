/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { GameListComponent } from "./game-list.component";

let component: GameListComponent;
let fixture: ComponentFixture<GameListComponent>;

describe("game component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [GameListComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(GameListComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
