/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { BotCommandComponent } from './bot-command.component';

let component: BotCommandComponent;
let fixture: ComponentFixture<BotCommandComponent>;

describe('bot-command component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ BotCommandComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(BotCommandComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});