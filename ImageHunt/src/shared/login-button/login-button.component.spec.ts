/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { LoginButtonComponent } from './login-button.component';

let component: LoginButtonComponent;
let fixture: ComponentFixture<LoginButtonComponent>;

describe('login-button component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ LoginButtonComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(LoginButtonComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});