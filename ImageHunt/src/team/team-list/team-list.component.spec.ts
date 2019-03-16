/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture } from "@angular/core/testing";
import { BrowserModule } from "@angular/platform-browser";
import { TeamListComponent } from "./team-list.component";
import { TeamService } from "../../shared/services/team.service";
let component: TeamListComponent;
let fixture: ComponentFixture<TeamListComponent>;
describe("team component",
  () => {
    beforeEach(async(() => {
      var teamServiceStub = {
        getTeams() {

        }
      };
      TestBed.configureTestingModule({
        declarations: [TeamListComponent],
        imports: [BrowserModule],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true },
          { provide: TeamService, userValue: teamServiceStub }
        ]
      });
      fixture = TestBed.createComponent(TeamListComponent);
      component = fixture.componentInstance;
    }));

    it("ngOnInit test",
      () => {
        var teamService = TestBed.get(TeamService);
        fixture.detectChanges();
      });
  });
