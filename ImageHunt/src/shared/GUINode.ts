import { Node } from "./node";

export class GUINode extends Node {
  selected: Boolean;

  constructor(node: Node) {
    super();
    this.id = node.id;
    this.nodeType = node.nodeType;

    this.name = this.nameWithIcon(node);
  }

  nameWithIcon(node: Node) {
    let nodeIcon = "";
    switch (node.nodeType) {
    case "FirstNode":
      nodeIcon = "&#xf11d;";
      break;
    case "TimerNode":
      nodeIcon = "&#xf017;";
      break;
    case "ObjectNode":
      nodeIcon = "&#xf1b2;";
      break;
    case "ChoiceNode":
      nodeIcon = "&#xf29c;";
      break;
    case "PictureNode":
      nodeIcon = "&#xf030;";
      break;
    case "LastNode":
      nodeIcon = "&#xf11e;";
      break;
    }
    return nodeIcon + " " + node.name;
  }
}
