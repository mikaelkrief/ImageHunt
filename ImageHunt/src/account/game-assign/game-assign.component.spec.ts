/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GameAssignComponent } from './game-assign.component';

let component: GameAssignComponent;
let fixture: ComponentFixture<GameAssignComponent>;

describe('game-assign component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GameAssignComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GameAssignComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});