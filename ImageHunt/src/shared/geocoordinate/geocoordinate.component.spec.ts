/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { GeocoordinateComponent } from './geocoordinate.component';

let component: GeocoordinateComponent;
let fixture: ComponentFixture<GeocoordinateComponent>;

describe('geocoordinate component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ GeocoordinateComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(GeocoordinateComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});