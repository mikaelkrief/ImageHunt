/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { GameDetailComponent } from "./game.detail.component";

let component: GameDetailComponent;
let fixture: ComponentFixture<GameDetailComponent>;

describe("gameDetail component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [GameDetailComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(GameDetailComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
