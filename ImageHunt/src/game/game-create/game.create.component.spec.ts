/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { GameCreateComponent } from "./game.create.component";

let component: GameCreateComponent;
let fixture: ComponentFixture<GameCreateComponent>;

describe("game.create component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [GameCreateComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(GameCreateComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
