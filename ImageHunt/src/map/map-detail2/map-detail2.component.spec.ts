/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { MapDetail2Component } from './map-detail2.component';

let component: MapDetail2Component;
let fixture: ComponentFixture<MapDetail2Component>;

describe('map-detail2 component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ MapDetail2Component ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(MapDetail2Component);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});