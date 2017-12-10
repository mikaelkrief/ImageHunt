import { Component, OnInit, Input } from '@angular/core';
import { Node } from "../../shared/node";
import {GameService} from "../../shared/services/game.service";
import { BsModalRef } from "ngx-bootstrap";
import { Observable } from "rxjs/Observable";
import {QuestionNode} from "../../shared/QuestionNode";
import {Answer} from "../../shared/answer";
import {AlertService} from "../../shared/services/alert.service";
import {QuestionNodeAnswerRelation} from "../../shared/QuestionNodeAnswerRelation";

@Component({
    selector: 'question-node',
    templateUrl: './question.node.component.html',
    styleUrls: ['./question.node.component.scss']
})
/** QuestionNode component*/
export class QuestionNodeComponent implements OnInit
{
  _gameId:number;
  get gameId(): number { return this._gameId; }

  set gameId(value: number) { this._gameId = value;
    this.loadNodes();
  }
  questionNodes: QuestionNode[];
  selectedNode: QuestionNode;
  selectedTargetNode: Node;
  selectedAnswer: Answer;
  _nodes: Node[];
  get nodes(): Node[] {
    return this._nodes;
  }
  @Input('nodes')
  set nodes(value: Node[]) {
    this._nodes = value;
  }
  availableNodes: Node[];
  linkBtnEnabled: boolean;
  unlinkBtnEnabled: boolean;
  answersNodeRelations: { answerId: number, answerName:string, nodeId: number, nodeName:string}[] =[];
    /** QuestionNode ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService, private _alertService: AlertService) { }

    /** Called by Angular after QuestionNode component initialized */
  ngOnInit(): void { this.linkBtnEnabled = this.unlinkBtnEnabled = false; }
  loadNodes() {
       this._gameService.getQuestionNodesOfGame(this.gameId)
        .subscribe(res => this.questionNodes = res);
      this._gameService.getGameById(this.gameId)
        .subscribe(res => {
          var n = res;
          this.nodes = n.nodes;
        });
    }
  nodeSelected(event) {
    this.availableNodes = this.nodes.filter(n => n.id !== this.selectedNode.nodeId
      && n.nodeType !== "FirstNode");
    }
  responseSelected() {
    if (this.selectedAnswer != null && this.selectedTargetNode != null) {
      if (this.answersNodeRelations.some(l => l.answerId === this.selectedAnswer.id &&
        l.nodeId === this.selectedTargetNode.id)) {
        this.unlinkBtnEnabled = true;
        this.linkBtnEnabled = false;
      } else {
        this.unlinkBtnEnabled = false;
        this.linkBtnEnabled = true;
      }
    } else {
      this.linkBtnEnabled = false;
      this.unlinkBtnEnabled = false;
    }
  }
  associateAnswerToNode() {
    if (this.selectedAnswer != null && this.selectedTargetNode != null) {
      this.answersNodeRelations.push({
        answerId: this.selectedAnswer.id, answerName: this.selectedAnswer.response,
        nodeId: this.selectedTargetNode.id, nodeName: this.selectedTargetNode.name
      });
      var orgNode = this.nodes.find(n => n.id == this.selectedNode.nodeId);
      orgNode.children.push(this.selectedTargetNode);
      this.unlinkBtnEnabled = true;
      this.linkBtnEnabled = false;
    }
  }
  dissociateAnswerToNode() {
    if (this.selectedAnswer != null && this.selectedTargetNode != null) {
      var index = this.answersNodeRelations.findIndex(l => l.answerId === this.selectedAnswer.id &&
        l.nodeId === this.selectedTargetNode.id);
      this.answersNodeRelations.splice(index, 1);
      this.unlinkBtnEnabled = false;
      this.linkBtnEnabled = true;
    }
  }
  saveRelations() {
    var relationAnswers = this.answersNodeRelations.map(a => new QuestionNodeAnswerRelation(this.selectedNode.nodeId, a.nodeId, a.answerId));
    this._gameService.addRelationAnswers(relationAnswers)
      .subscribe(res => {
        this._alertService.sendAlert(`Les relations du noeud ${this.selectedNode.name} ont été sauvegardées`, "success", 5000);
        this.answersNodeRelations = [];
      });
    this.bsModalRef.hide();
  }
}
