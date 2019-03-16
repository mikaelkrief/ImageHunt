/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GameActionListComponent } from './game-action-list.component';

let component: GameActionListComponent;
let fixture: ComponentFixture<GameActionListComponent>;

describe('GameActionList component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GameActionListComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GameActionListComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
