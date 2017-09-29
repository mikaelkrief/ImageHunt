import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef, TabsetComponent } from "ngx-bootstrap";
import { NgForm } from "@angular/forms";
import { Node } from "../../shared/node";

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
  @Output() newNode = new EventEmitter<Node>();
    /** node-create ctor */
  constructor(public bsModalRef: BsModalRef) { }

    /** Called by Angular after node-create component initialized */
  ngOnInit(): void { }

  createNode(form: NgForm) {
    var node: Node = {
      id: 0,
      image:null,
      nodeType : form.value.nodeType,
      name: form.value.name,
      latitude: this.latitude,
      longitude: this.longitude,
      children:null
    };
    this.newNode.emit(node);
  }
}
