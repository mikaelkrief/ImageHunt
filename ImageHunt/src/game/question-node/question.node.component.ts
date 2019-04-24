import { Component, OnInit, Input } from '@angular/core';
import { Node } from "../../shared/node";
import {GameService} from "../../shared/services/game.service";
import { BsModalRef } from "ngx-bootstrap";
import { Observable } from "rxjs";
import {QuestionNode} from "../../shared/QuestionNode";
import {Answer} from "../../shared/answer";
import {AlertService} from "../../shared/services/alert.service";
import {QuestionNodeAnswerRelation} from "../../shared/QuestionNodeAnswerRelation";
import { Game } from '../../shared/game';

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
         .subscribe((nodes: QuestionNode[]) => {
           this.questionNodes = nodes;
           this.fillAnswerNodeRelations();
         });
      this._gameService.getGameById(this.gameId)
        .subscribe((game:Game) => {
          this.nodes = game.nodes;
        });
  }
  fillAnswerNodeRelations() {
    for (var questionNode of this.questionNodes) {
      for (var answer of questionNode.answers) {
        this.answersNodeRelations.push({
          answerId: answer.id,
          answerName: answer.response,
          nodeId: answer.nodeId,
          nodeName: ''
        });
      }
    }
  }
  nodeSelected(event) {
    this.availableNodes = this.nodes.filter(n => n.id !== this.selectedNode.id
      && n.nodeType !== "FirstNode");
    }
  answerSelected() {
    this.selectedTargetNode = undefined;
    if (!this.selectedAnswer) {
      this.linkBtnEnabled = false;
      this.unlinkBtnEnabled = false;
      return;
    }
    if (this.selectedAnswer) {
        if (this.answersNodeRelations.some(l => l.answerId === this.selectedAnswer.id)) {
          this.selectedTargetNode = this.availableNodes.find(n => n.id === this.selectedAnswer.nodeId);
          this.unlinkBtnEnabled = true;
          this.linkBtnEnabled = false;
        } else {
          this.unlinkBtnEnabled = false;
          this.linkBtnEnabled = false;
        }
      } 
     
  }
  targetNodeSelected() {
    if (!this.selectedAnswer || !this.selectedTargetNode) {
      this.linkBtnEnabled = false;
      this.unlinkBtnEnabled = false;
      return;
    }
    if (this.selectedTargetNode) {
      if (this.answersNodeRelations.some(l => l.answerId === this.selectedAnswer.id && l.nodeId === this.selectedTargetNode.id)) {
        this.linkBtnEnabled = false;
        this.unlinkBtnEnabled = true;

      } else {
        this.linkBtnEnabled = true;
        this.unlinkBtnEnabled = false;
      }
    }
  }
  associateAnswerToNode() {
    if (this.selectedAnswer != null && this.selectedTargetNode != null) {
      this.answersNodeRelations.push({
        answerId: this.selectedAnswer.id, answerName: this.selectedAnswer.response,
        nodeId: this.selectedTargetNode.id, nodeName: this.selectedTargetNode.name
      });
      this.selectedAnswer.nodeId = this.selectedTargetNode.id;
      this.unlinkBtnEnabled = true;
      this.linkBtnEnabled = false;
    }
  }
  dissociateAnswerToNode() {
    if (this.selectedAnswer != null && this.selectedTargetNode != null) {
      var index = this.answersNodeRelations.findIndex(l => l.answerId === this.selectedAnswer.id &&
        l.nodeId === this.selectedTargetNode.id);
      this.answersNodeRelations.splice(index, 1);
      this.selectedTargetNode = undefined;
      this.selectedAnswer.nodeId = undefined;
      this.unlinkBtnEnabled = false;
      this.linkBtnEnabled = false;
    }
  }
  saveRelations() {
    var relationAnswers = this.answersNodeRelations.map(a => new QuestionNodeAnswerRelation(this.selectedNode.id, a.nodeId, a.answerId));
    this._gameService.addRelationAnswers(relationAnswers)
      .subscribe(res => {
        this._alertService.sendAlert(`Les relations du noeud ${this.selectedNode.name} ont été sauvegardées`, "success", 5000);
        this.answersNodeRelations = [];
      });
    this.bsModalRef.hide();
  }
}
