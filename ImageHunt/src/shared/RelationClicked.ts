import { Node } from "./node";

export class RelationClicked {
  constructor(public node1: Node, public node2: Node, public mouseEvent: MouseEvent) {}

}
