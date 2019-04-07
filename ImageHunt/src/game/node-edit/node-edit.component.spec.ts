/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { NodeEditComponent } from './node-edit.component';

let component: NodeEditComponent;
let fixture: ComponentFixture<NodeEditComponent>;

describe('node-edit component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ NodeEditComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(NodeEditComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});