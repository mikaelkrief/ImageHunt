import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { BsModalRef } from "ngx-bootstrap";
import { Node } from "../../shared/Node"
import { GameService } from "../../shared/services/game.service";

@Component({
  selector: 'node-relation',
  templateUrl: './node.relation.component.html',
  styleUrls: ['./node.relation.component.scss']
})
/** node.relation component*/
export class NodeRelationComponent implements OnInit {
  _nodes: Node[];
  get nodes(): Node[] {
    return this._nodes;
  }
  @Input('nodes')
  set nodes(value: Node[]) {
    this._nodes = value;
    this.updateNodes();
  }
  updateNodes() {
    this.parentNodes = this.nodes.filter(n => n.nodeType !== "LastNode");
  }
  parentNodes: Node[];
  childrenNodes: Node[];
  availableNodes: Node[];
  /** node.relation ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService) { }

  /** Called by Angular after node.relation component initialized */
  ngOnInit(): void {

  }

  parentSelected(node: Node): void {
    this.childrenNodes = node.children;
    this.availableNodes = this.nodes.filter(n => n.nodeType !== "FirstNode")
      .filter(n => !this.childrenNodes.find(n2 => n2 === n));
    this.addNodeDisabled = (node.nodeType !== "QuestionNode" && node.children.length > 0);
    this.removeNodeDisabled = node.children.length === 0;
  }
  addNodeDisabled: boolean;
  removeNodeDisabled: boolean;
  addChildren() {
    if (this.selectedParent && this.selectedAvailable) {
      this.selectedParent.children.push(this.selectedAvailable);
      this.availableNodes.splice(this.availableNodes.indexOf(this.selectedAvailable), 1);
      this.addNodeDisabled = this.removeNodeDisabled = true;
    }
  }
  removeChildren() {
    if (this.selectedParent && this.selectedChildren) {
      this.selectedParent.children.splice(this.selectedParent.children.indexOf(this.selectedChildren), 1);
      this.availableNodes.push(this.selectedChildren);
      this.addNodeDisabled = this.removeNodeDisabled = true;
    }
  }
  selectedParent: Node;
  selectedChildren: Node;
  selectedAvailable: Node;
}
