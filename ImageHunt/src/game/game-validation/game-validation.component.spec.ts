/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GameValidationComponent } from './game-validation.component';

let component: GameValidationComponent;
let fixture: ComponentFixture<GameValidationComponent>;

describe('game-validation component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GameValidationComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GameValidationComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});