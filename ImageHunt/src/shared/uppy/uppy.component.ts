import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    selector: 'uppy',
    templateUrl: './uppy.component.html',
    styleUrls: ['./uppy.component.scss']
})
/** uppy component*/
export class UppyComponent {
    /** uppy ctor */
  constructor(public bsModalRef: BsModalRef) {

    }
}
