import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef, TabsetComponent } from "ngx-bootstrap";
import { NgForm } from "@angular/forms";
import { Node } from "../../shared/node";
import {NodeRequest} from '../../shared/nodeRequest';

@Component({
    selector: 'node-create',
    templateUrl: './node.create.component.html',
    styleUrls: ['./node.create.component.scss']
})
/** node-create component*/
export class NodeCreateComponent implements OnInit
{
  public latitude: number;
  public longitude: number;
  @Output() newNode = new EventEmitter<NodeRequest>();
    /** node-create ctor */
  constructor(public bsModalRef: BsModalRef) { }

    /** Called by Angular after node-create component initialized */
  ngOnInit(): void { }

  createNode(form: NgForm) {
    var node: NodeRequest = {
      nodeType : form.value.nodeType,
      name: form.value.name,
      latitude: this.latitude,
      longitude: this.longitude,
      duration: form.value.duration === '' ? 0 : form.value.duration,
      action: form.value.action,
      question: form.value.question,
      points: form.value.nbPoints === '' ? 0 : form.value.nbPoints,
      answers: null
    };
    // Add answers
    if (form.value.nodeType === 'QuestionNode') {
      node.answers = new Array();
      for (let i = 0; i < this.answers.length; i++) {
        node.answers.push({ response:this.answers[i], correct:i === +this.correctAnswer});
      }
    }
    this.newNode.emit(node);
  }
  addAnswer(newAnswer:NgForm) {
    if (this.answers == null)
      this.answers = [];
    this.answers.push(newAnswer.value.answer);
    newAnswer.reset();
  }
  correctAnswer:number;
  answers: string[];
}
