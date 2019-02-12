/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GoogleSigninComponent } from './google-signin.component';

let component: GoogleSigninComponent;
let fixture: ComponentFixture<GoogleSigninComponent>;

describe('google-signin component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GoogleSigninComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GoogleSigninComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});