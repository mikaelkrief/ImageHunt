/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { TeamCreateComponent } from './team-create.component';

let component: TeamCreateComponent;
let fixture: ComponentFixture<TeamCreateComponent>;

describe('teamCreate component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ TeamCreateComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(TeamCreateComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});