import { Component, OnInit, Input } from '@angular/core';
import { Node } from "../../shared/node";
import {GUINode} from "../../shared/GUINode";

@Component({
    selector: 'node-list',
    templateUrl: './node.list.component.html',
    styleUrls: ['./node.list.component.scss']
})
/** node-list component*/
export class NodeListComponent implements OnInit
{
  @Input() nodes: Node[];
  guiNodes: GUINode[];
    /** node-list ctor */
    constructor() { }

    /** Called by Angular after node-list component initialized */
    ngOnInit(): void {
      
    }
    ngOnChanges(changes) {
      if (this.nodes != null) {
        this.guiNodes = this.nodes.map(n => new GUINode(n));
      }
    }
  nodeIcon(node: Node) {
    switch (node.nodeType) {
    case "FirstNode":
        return "fa-flag-o";
    case "LastNode":
      return "fa-flag-checkered";
    case "ObjectNode":
      return "fa-cube";
    case "TimerNode":
      return "fa-clock-o";
    case "QuestionNode":
      return "fa-question-circle-o";
    case "PictureNode":
      return "fa-camera";
    default:
    }
  }
  nodeClick(node: GUINode) {
    this.guiNodes.forEach(n=>n.selected = false);
    node.selected = true;
  }
}
