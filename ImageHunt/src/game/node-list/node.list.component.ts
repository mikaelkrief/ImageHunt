import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Node } from "../../shared/node";
import {GUINode} from "../../shared/GUINode";

@Component({
    selector: 'node-list',
    templateUrl: './node.list.component.html',
    styleUrls: ['./node.list.component.scss']
})
/** node-list component*/
export class NodeListComponent implements OnInit, OnChanges
{
  @Input() nodes: Node[];
  guiNodes: GUINode[];
    /** node-list ctor */
    constructor() { }

    /** Called by Angular after node-list component initialized */
    ngOnInit(): void {
      
    }
    ngOnChanges(changes: SimpleChanges) {
      if (this.nodes != null) {
        this.guiNodes = this.nodes.map(n => new GUINode(n));
      }
    }
}
