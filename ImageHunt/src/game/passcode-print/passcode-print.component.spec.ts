/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { PasscodePrintComponent } from './passcode-print.component';

let component: PasscodePrintComponent;
let fixture: ComponentFixture<PasscodePrintComponent>;

describe('passcode-print component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ PasscodePrintComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(PasscodePrintComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});