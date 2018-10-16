import { Component, OnInit, Input, Output, EventEmitter, AfterViewInit, AfterContentChecked, AfterContentInit } from '@angular/core';
import * as L from 'leaflet';
import 'leaflet-polylinedecorator';

import { GameService } from '../../shared/services/game.service';
import { GeoPoint } from '../../shared/GeoPoint';
import { NodeClicked } from '../../shared/NodeClicked';
import { RelationClicked } from '../../shared/RelationClicked';
import { NodeRelation } from '../../shared/NodeRelation';
import { Observable } from 'rxjs';
import { Game } from '../../shared/game';
import { Node } from '../../shared/node';
import { AlertService } from '../../shared/services/alert.service';
import { NodeDragged } from "../../shared/NodeDragged";

class NodeMarker extends L.Marker {
  node: Node;
}

@Component({
  selector: 'map-detail3',
  templateUrl: './map-detail3.component.html',
  styleUrls: ['./map-detail3.component.scss']
})
/** map-detail3 component*/
export class MapDetail3Component implements OnInit {
  @Input() gameId: number;
  @Input() newNodesRelation: GeoPoint[];
  @Input() nodeMode: string;
  @Input() filterNode: string[];
  @Input() nodeRelations: NodeRelation[];
  @Input() latCenter: number;
  @Input() lngCenter: number;
  @Input() zoom: number;
  @Input() nodes: Node[];
  @Input() editable: boolean;

  @Output() mapClicked = new EventEmitter();
  @Output() nodeClicked = new EventEmitter<NodeClicked>();
  @Output() nodeRightClicked = new EventEmitter<NodeClicked>();
  @Output() nodeDragged = new EventEmitter<NodeDragged>();
  @Output() relationRightClicked = new EventEmitter<RelationClicked>();
  @Output() newRelation = new EventEmitter<NodeRelation>();
  @Output() zoomChange = new EventEmitter<number>();
  map: any;
  markers: any[] = [];
  polylines: L.Polyline[] = [];

  ngOnInit(): void {
    this.map = L.map("MapDetail")
      .setView([0, 0], 12);

    L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
      attribution: 'ImageHunt'
    }).addTo(this.map);
    this.updateMap();


  }

  /** map-detail3 ctor */
  constructor() {
    this.latCenter = 0;
    this.lngCenter = 0;
    this.zoom = 1;

  }
  clearMap() {
    for (let i in this.map._layers) {
      if (this.map._layers[i] !== undefined) {
        if (this.map._layers[i]._paths != undefined) {
          try {
            this.map.removeLayer(this.map._layers[i]);
            continue;
          } catch (e) {
            console.log("problem with " + e + this.map._layers[i]);
          }
        }
        if (this.map._layers[i].node !== undefined) {
          this.map.removeLayer(this.map._layers[i]);
          continue;
        }
      }
    }
  }
  updateMap() {

    if (this.latCenter) {
      this.map.setView(new L.LatLng(this.latCenter, this.lngCenter), this.zoom);
      this.map.on('click', event => this.mapClicked.emit(event));
      this.createMarkers();
      this.createRelations();
      this.createNewRelations();
      //this.createContextMenu();
    }
  }


  createRelations() {

    if (this.nodes) {
      this.nodes.forEach(node => {
        if (node.children) {
          node.children.forEach(children => {
            const polyline = L.polyline(
              [
                [node.latitude, node.longitude],
                [children.latitude, children.longitude]
              ],
              { color: 'Blue', weight: 2 }
            );
            //polyline.addTo(this.map);
            this.polylines.push(polyline);
            const symbol = L.Symbol.arrowHead({ pixelSize: 10, pathOptions: { fillOpacity: 1, weight: 0 } });
            const decorator = L.polylineDecorator(polyline,
              {
                patterns:
                  [
                    { offset: 0, repeat: 20, symbol: symbol }
                  ]
              });
            decorator.addTo(this.map);
          });
        }
      });
    }
  }
  createNewRelations() {
    if (this.newRelation !== undefined) {
      this.newRelation.forEach(relation => {
        const firstNode = this.nodes.find(n => n.id === relation.nodeId);
        const secondNode = this.nodes.find(n => n.id === relation.childNodeId);
        const polyline = L.polyline(
          [
            [firstNode.latitude, firstNode.longitude],
            [secondNode.latitude, secondNode.longitude]
          ],
          { color: 'Red', weight: 2 }
        );
        polyline.addTo(this.map);

      });
    }
  }
  createMarkers() {
    this.nodes.forEach(node => {
      const icon = L.icon({
        iconUrl: this.getIconForNodeType(node.nodeType),
        iconSize: [32, 32],
        iconAnchor: [16, 16]
      });
      const marker = new NodeMarker([node.latitude, node.longitude],
        { icon: icon, title: node.name, draggable: this.editable });
      marker.node = node;
      marker.addTo(this.map);
      marker.on('click', event => this.onNodeClick(event));
      marker.on('dragend', event => this.onNodeDragged(event));

      marker.on("contextmenu", event => this.markerRightClick(event));
      //marker.addEventListener("dragend")
      this.markers.push({ marker, node });
    });
  }
  getIconForNodeType(nodeType: string): string {
    switch (nodeType) {
      case 'TimerNode':
        return 'assets/timerNode.png';
      case 'PictureNode':
        return 'assets/pictureNode.png';
      case 'FirstNode':
        return 'assets/startNode.png';
      case 'LastNode':
        return 'assets/endNode.png';
      case 'QuestionNode':
        return 'assets/questionNode.png';
      case 'ObjectNode':
        return 'assets/objectNode.png';
      default:
        return null;
    }
  }
  isFirstClick: boolean = true;
  firstNode: Node;
  secondNode: Node;

  resetNodeClick() {
    this.firstNode = null;
    this.isFirstClick = true;
  }

  onNodeClick(leafletEvent: L.LeafletEvent): void {
    let node = leafletEvent.target.node;
    let nClicked: NodeClicked;
    if (node.nodeType !== "PictureNode") {
      if (this.isFirstClick) {
        this.firstNode = node;
        this.isFirstClick = false;
        nClicked = new NodeClicked(node, 1, null);
      } else {
        this.secondNode = node;
        this.isFirstClick = true;
        nClicked = new NodeClicked(node, 2, null);
        this.newRelation.emit({ nodeId: this.firstNode.id, childNodeId: this.secondNode.id });
      }
    } else {
      nClicked = new NodeClicked(node, 1, null);
    }
    this.nodeClicked.emit(nClicked);
  }


  markerRightClick(leafletEvent: L.LeafletEvent): void {
    let node = leafletEvent.target.node;
    //  this.nodeMenuItems = [
    //    { label: 'Modifier', icon: 'fa-edit', disabled: true },
    //    { label: 'Effacer', icon: 'fa-trash', command: event => this.deleteNode(node.id) },
    //  ];
    //  if (node.nodeType === 'QuestionNode') {
    //    this.nodeMenuItems.push({
    //      label: 'Editer les relations',
    //      automationId: node.id,
    //      //command: event => this.editNodeAnswers()
    //    });
    //  }
    //  this.markerContextMenu.show(event.Ia);
    //  this.nodeRightClicked.emit(new NodeClicked(node, 0, event.Ia));

  }

  onNodeDragged(leafletEvent: L.LeafletEvent): void {
    var newPosition = leafletEvent.target.getLatLng();
    var node = leafletEvent.target.node;
    node.latitude = newPosition.lat;
    node.longitude = newPosition.lng;
    this.clearMap();

    this.nodeDragged.emit({ node, newPosition});
  }

}
