import { Component, OnInit, Input, AfterViewInit, ViewChild, EventEmitter, Output, AfterViewChecked } from '@angular/core';
import { Node } from 'shared/node';
import * as L from 'leaflet';
import { BsModalRef, BsModalService, TabDirective } from 'ngx-bootstrap';
import { GameService } from 'services/game.service';
import { AlertService } from "services/alert.service";
import { NgForm } from '@angular/forms';
import { UploadImageComponent } from 'shared/upload-image/upload-image.component';
import { NodeRequest } from "shared/nodeRequest";

class NodeMarker extends L.Marker {
  node: Node;
}

@Component({
  selector: 'node-edit',
  templateUrl: './node-edit.component.html',
  styleUrls: ['./node-edit.component.scss']
})

/** node-edit component*/
export class NodeEditComponent implements OnInit, AfterViewInit, AfterViewChecked {
  pictureId: number;

  ngAfterViewInit(): void {
    this.setMap();
  }
  ngAfterViewChecked(): void {
    this.map.invalidateSize();
  }
  nodeMarker: any;

  ngOnInit(): void {

  }

  @Input('gameId')
  gameId: number;

  @Input('node')
  set node(value: Node) {
    this._node = value;
    this.modeTitle = !this._node.id ? "Create" : "Edit";
  }

  get node() {
    return this._node;
  }

  @Output('node')
  nodeEmit = new EventEmitter<Node>();
  _node: Node;

  public createMode: boolean;
  modeTitle: string;
  nodeTypes = [
    { color: "red", value: "FirstNode", label: "Start", icon: "fa fa-flag" },
    { color: "cadetblue", value: "WaypointNode", label: "Waypoint", icon: "fas fa-map-marker-alt" },
    { color: "green", value: "TimerNode", label: "Timer", icon: "fas fa-clock" },
    { color: "purple", value: "ObjectNode", label: "Action", icon: "fas fa-running" },
    { color: "violet", value: "HiddenNode", label: "Hidden", icon: "fas fa-mask" },
    { color: "green", value: "ChoiceNode", label: "Choice", icon: "fas fa-list-ul" },
    { color: "blue", value: "QuestionNode", label: "Question", icon: "fas fa-question" },
    { color: "blue", value: "PictureNode", label: "Picture", icon: "fas fa-camera" },
    { color: "purple", value: "BonusNode", label: "Bonus", icon: "fas fa-gift" },
    { color: "green", value: "LastNode", label: "End", icon: "fas fa-flag-checkered" },
  ];
  map: any;

  /** node-edit ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService, private _modalService: BsModalService) {
    this._node = new Node();
    this._node.image = { id: 0, name: '' }
    this.modeTitle = "Create";
  }

  setMap() {
    this.map = L.map("map")
      .setView([this._node.latitude, this._node.longitude], 15);
    L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png',
      {
        attribution: 'Pixhint'
      }).addTo(this.map);
    const icon = L.divIcon({ className: "fas fa-2x fa-bullseye" });

    this.nodeMarker = new NodeMarker([this._node.latitude, this._node.longitude],
      { icon: icon, title: this._node.name, draggable: true });
    this.nodeMarker.node = this._node;
    this.nodeMarker.addTo(this.map);
    this.nodeMarker.on('dragend', event => this.onNodeDragged(event));
  }

  onNodeDragged(leafletEvent: L.LeafletEvent): void {
    var newPosition = leafletEvent.target.getLatLng();
    this._node.latitude = newPosition.lat;
    this._node.longitude = newPosition.lng;
  }
  onSelectLocation(data: TabDirective) {
    this.map.invalidateSize();
  }
  modalRef: any;

  uploadImage() {
    this.modalRef = this._modalService.show(UploadImageComponent, { ignoreBackdropClick: true });
    this.modalRef.content.pictureId.subscribe(id => this._node.image = { pictureId: id, name: '' });
  }

  saveChanges(form: NgForm) {
    this.nodeEmit.emit(this.node);
    this.bsModalRef.hide();
  }
}
