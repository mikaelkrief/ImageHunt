import {Node as Node1} from "./node";

export class GUINode extends Node1 {
  selected: Boolean;
  constructor(node: Node1) {
    super();
    this.id = node.id;
    this.nodeType = node.nodeType;
    this.name = node.name;
  }
}
