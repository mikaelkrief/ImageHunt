import { Component, TemplateRef, OnInit, Input } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { GeoPoint } from '../../shared/GeoPoint';
import { Node } from '../../shared/node';
import {GameService} from "../../shared/services/game.service";

@Component({
    selector: 'app-image-node-edit',
    templateUrl: './image-node-edit.component.html',
    styleUrls: ['./image-node-edit.component.scss']
})
/** imageNode-edit component*/
export class ImageNodeEditComponent implements OnInit{
  nodePosition: GeoPoint;
  @Input('node')
  set node(value: Node) {
    //this._node = value;
    this._gameService.getNodeById(value.id)
      .subscribe(data => this._node = data.json());
  }
  _node: Node;
  ngOnInit(): void {
    
  }
    /** imageNode-edit ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService) {
    this.nodePosition = new GeoPoint;
  }
  saveChanges(form) {
    this._node.name = form.form.value.name;
  }
}
