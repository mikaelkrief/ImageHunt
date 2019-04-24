/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GameActionDetailComponent } from './game-action-detail.component';

let component: GameActionDetailComponent;
let fixture: ComponentFixture<GameActionDetailComponent>;

describe('game-action-detail component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GameActionDetailComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GameActionDetailComponent);
        component = fixture.componentInstance;
    }));

  it('should no answer selected no answer selected and buttons disabled', async(() => {
    //component.selectedAnswer = component.selectedTargetNode = undefined;
    //expect(component.linkBtnEnabled).toEqual(false);
    //expect(component.unlinkBtnEnabled).toEqual(false);

    }));
});
