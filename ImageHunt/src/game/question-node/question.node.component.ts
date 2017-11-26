import { Component, OnInit, Input } from '@angular/core';
import { Node } from "../../shared/node";
import {GameService} from "../../shared/services/game.service";
import { BsModalRef } from "ngx-bootstrap";
import { Observable } from "rxjs/Observable";
import {QuestionNode} from "../../shared/QuestionNode";
import {Answer} from "../../shared/answer";

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

  set gameId(value: number) { this._gameId = value; this.loadNodes()}
  questionNodes: QuestionNode[];
  selectedNode: QuestionNode;
  selectedTargetNode: Node;
  selectedAnswer: Answer;
  nodes: Node[];
  availableNodes: Node[];
  linkBtnEnabled: boolean;
  unlinkBtnEnabled: boolean;
  answersNodeRelations: {answerId:number, nodeId:number}[] =[];
    /** QuestionNode ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService:GameService) { }

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
  //availableNodeSelected() {
  //  if (this.selectedAnswer != null && this.selectedTargetNode != null) {
  //    this.linkBtnEnabled = true;

  //  } else {
  //    this.linkBtnEnabled = false;
  //  }
  //}
  associateAnswerToNode() {
    if (this.selectedAnswer != null && this.selectedTargetNode != null) {
      this.answersNodeRelations.push({ answerId: this.selectedAnswer.id, nodeId: this.selectedTargetNode.id });
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
}
