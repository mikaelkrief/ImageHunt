import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef, TabsetComponent } from 'ngx-bootstrap';
import { NgForm } from '@angular/forms';
import { Node } from '../../shared/node';
import {NodeRequest} from '../../shared/nodeRequest';

@Component({
    selector: 'node-create',
    templateUrl: './node.create.component.html',
    styleUrls: ['./node.create.component.scss']
})
/** node-create component*/
export class NodeCreateComponent implements OnInit {
  public latitude: number;
  public longitude: number;
  correctAnswer: number;
  choices: string[];
  @Output() newNode = new EventEmitter<NodeRequest>();
    /** node-create ctor */
  constructor(public bsModalRef: BsModalRef) { }

    /** Called by Angular after node-create component initialized */
  ngOnInit(): void { }

  createNode(form: NgForm) {
    const node: NodeRequest = {
      nodeType : form.value.nodeType,
      name: form.value.name,
      latitude: this.latitude,
      longitude: this.longitude,
      duration: form.value.duration === '' ? 0 : form.value.duration,
      action: form.value.action,
      question: form.value.question,
      answer: form.value.answer,
      points: form.value.nbPoints === '' ? 0 : form.value.nbPoints,
      choices: null,
      password: form.value.password,
      hint: form.value.hint,
      bonus: form.value.bonustype === '' ? 0 : form.value.bonustype,
      location: form.value.location

    };
    // Add answers
    if (form.value.nodeType === 'ChoiceNode') {
      node.choices = new Array();
      node.question = form.value.choiceQuestion;
      for (let i = 0; i < this.choices.length; i++) {
        node.choices.push({ response: this.choices[i], correct: i === +this.correctAnswer});
      }
    }
    this.newNode.emit(node);
  }
  addChoice(newChoice: NgForm) {
    if (this.choices == null) {
      this.choices = [];
    }
    this.choices.push(newChoice.value.choice);
    newChoice.reset();
  }
}
