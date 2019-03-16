/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { UploadImageComponent } from './upload-image.component';

let component: UploadImageComponent;
let fixture: ComponentFixture<UploadImageComponent>;

describe('upload-image component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ UploadImageComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(UploadImageComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});