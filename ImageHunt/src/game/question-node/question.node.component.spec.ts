/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { QuestionNodeComponent } from './question.node.component';

let component: QuestionNodeComponent;
let fixture: ComponentFixture<QuestionNodeComponent>;

describe('QuestionNode component', () =>
{
    beforeEach(async(() =>
    {
        TestBed.configureTestingModule({
            declarations: [QuestionNodeComponent],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(QuestionNodeComponent);
        component = fixture.componentInstance;
    }));

    it('Enable Associate Button', async(() => {

      // Act
      fixture.nativeElement.responseSelected();
    }));
});
