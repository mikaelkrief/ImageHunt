/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { TeamFollowComponent } from "./team-follow.component";

let component: TeamFollowComponent;
let fixture: ComponentFixture<TeamFollowComponent>;

describe("team-follow component",
  () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [TeamFollowComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true }
        ]
      });
      fixture = TestBed.createComponent(TeamFollowComponent);
      component = fixture.componentInstance;
    }));

    it("should do something",
      async(() => {
        expect(true).toEqual(true);
      }));
  });
