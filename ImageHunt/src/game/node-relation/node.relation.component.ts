import { Node } from "../../shared/node"
import { GameService } from "../../shared/services/game.service";
import { AlertService } from "../../shared/services/alert.service";

@Component({
  selector: "node-relation",
  templateUrl: "./node.relation.component.html",
  styleUrls: ["./node.relation.component.scss"]
})
/** node.relation component*/
export class NodeRelationComponent implements OnInit {
  _nodes: Node[];

  get nodes(): Node[] {
    return this._nodes;
  }

  @Input("nodes")
  set nodes(value: Node[]) {
    this._nodes = value;
    this.updateNodes();
  }

  updateNodes() {
    this.parentNodes = this.nodes.filter(n => n.nodeType !== "LastNode" && n.nodeType !== "ChoiceNode");
    this.selectedParent = this.parentNodes[0];
    this.parentSelected(this.selectedParent);
  }

  parentNodes: Node[];
  childrenNodes: Node[];
  availableNodes: Node[];

  /** node.relation ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService, private _alertService: AlertService) {}

  /** Called by Angular after node.relation component initialized */
  ngOnInit(): void {
  }

  parentSelected(node: Node): void {
    // Set the node children
    this.childrenNodes = node.children;
    // Set the available nodes
    this.availableNodes = this.nodes
      .filter(n => n.nodeType !== "FirstNode")
      .filter(n => !this.childrenNodes.find(n2 => n2 === n))
      .filter(n => n !== node);
    this.addNodeDisabled = (node.nodeType !== "ChoiceNode" && node.children.length > 0);
    this.removeNodeDisabled = node.children.length === 0;
  }

  addNodeDisabled: boolean;
  removeNodeDisabled: boolean;

  addChildren() {
    if (this.selectedParent && this.selectedAvailable) {
      this.selectedParent.children.push(this.selectedAvailable);
      this.availableNodes.splice(this.availableNodes.indexOf(this.selectedAvailable), 1);
      this.addNodeDisabled = this.removeNodeDisabled = true;
      this._gameService.addRelation(this.selectedParent.id, this.selectedAvailable.id, 0)
        .subscribe(res => this._alertService.sendAlert(
          `Le noeud ${this.selectedAvailable.name} a été ajouté aux enfants de ${this.selectedParent.name}`,
          "success",
          5000));
    }
  }

  removeChildren() {
    if (this.selectedParent && this.selectedChildren) {
      this.selectedParent.children.splice(this.selectedParent.children.indexOf(this.selectedChildren), 1);
      this.availableNodes.push(this.selectedChildren);
      this.addNodeDisabled = this.removeNodeDisabled = true;
      this._gameService.removeRelation(this.selectedParent.id, this.selectedChildren.id)
        .subscribe(res => this._alertService.sendAlert(
          `Le noeud ${this.selectedChildren.name} a été ôté aux enfants de ${this.selectedParent.name}`,
          "warning",
          5000));
    }
  }

  selectedParent: Node;
  selectedChildren: Node;
  selectedAvailable: Node;
}
