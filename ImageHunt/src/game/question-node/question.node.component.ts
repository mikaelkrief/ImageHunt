import { Component, OnInit, Input } from '@angular/core';
import { Node } from "../../shared/Node";
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
  selectedAnswer: Answer;
  nodes:Node[];
    /** QuestionNode ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService:GameService) { }

    /** Called by Angular after QuestionNode component initialized */
    ngOnInit(): void { }
    loadNodes() {
      this._gameService.getQuestionNodesOfGame(this.gameId)
        .subscribe(res => this.questionNodes = res);
      this._gameService.getGameById(this.gameId)
        .subscribe(res => {
          var n = res;
          this.nodes = n;
        });
    }
  nodeSelected(node: Node) {
    
  }
}
