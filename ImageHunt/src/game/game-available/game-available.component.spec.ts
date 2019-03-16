/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GameAvailableComponent } from './game-available.component';

let component: GameAvailableComponent;
let fixture: ComponentFixture<GameAvailableComponent>;

describe('game-available component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GameAvailableComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GameAvailableComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});