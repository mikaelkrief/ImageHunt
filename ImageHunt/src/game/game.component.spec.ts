/// <reference path="../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GameComponent } from './game.component';

let component: GameComponent;
let fixture: ComponentFixture<GameComponent>;

describe('game component', () =>
{
    beforeEach(async(() =>
    {
        TestBed.configureTestingModule({
            declarations: [GameComponent],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GameComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() =>
    {
        expect(true).toEqual(true);
    }));
});