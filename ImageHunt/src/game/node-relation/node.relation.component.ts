import { Component, OnInit, Input } from '@angular/core';
import { BsModalRef } from "ngx-bootstrap";

@Component({
    selector: 'node-relation',
    templateUrl: './node.relation.component.html',
    styleUrls: ['./node.relation.component.scss']
})
/** node.relation component*/
export class NodeRelationComponent implements OnInit
{
  @Input() nodes: Node[];
    /** node.relation ctor */
  constructor(public bsModalRef: BsModalRef) { }

    /** Called by Angular after node.relation component initialized */
    ngOnInit(): void { }
}
