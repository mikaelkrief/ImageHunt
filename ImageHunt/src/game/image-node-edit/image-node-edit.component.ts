import { Component, TemplateRef, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { GeoPoint } from '../../shared/GeoPoint';

@Component({
    selector: 'app-image-node-edit',
    templateUrl: './image-node-edit.component.html',
    styleUrls: ['./image-node-edit.component.scss']
})
/** imageNode-edit component*/
export class ImageNodeEditComponent implements OnInit{
  nodePosition: GeoPoint;
  node: Node;
  ngOnInit(): void {
  }
    /** imageNode-edit ctor */
  constructor(public bsModalRef: BsModalRef) {
    this.nodePosition = new GeoPoint;
  }
}
