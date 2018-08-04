import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import * as L from 'leaflet';

import { GameService } from '../../shared/services/game.service';
import { GeoPoint } from '../../shared/GeoPoint';
import { NodeClicked } from '../../shared/NodeClicked';
import { RelationClicked } from '../../shared/RelationClicked';
import { NodeRelation } from '../../shared/NodeRelation';
import { Observable } from 'rxjs/Observable';
import { Game } from '../../shared/game';
import { Node } from '../../shared/node';
import { AlertService } from '../../shared/services/alert.service';

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
  @Output() mapClicked = new EventEmitter();
  @Output() nodeClicked = new EventEmitter<NodeClicked>();
  @Output() nodeRightClicked = new EventEmitter<NodeClicked>();
  @Output() relationRightClicked = new EventEmitter<RelationClicked>();
  @Output() newRelation = new EventEmitter<NodeRelation>();
  @Output() zoomChange = new EventEmitter<number>();
  game: Game;
  nodeRelations: NodeRelation[];
  map: any;
  markers: any[] = [];
  nodes: Node[];

  ngOnInit(): void {
    this.map = L.map("MapDetail")
      .setView([0, 0], 12);

    L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
      attribution: 'ImageHunt'
    }).addTo(this.map);

  }
  /** map-detail3 ctor */
  constructor(private _gameService: GameService, private _alertService: AlertService) {

  }
  updateMap() {

    Observable.forkJoin(
        this._gameService.getGameById(this.gameId),
        this._gameService.getNodeRelations(this.gameId))

      .subscribe(([game, relations]) => {
        this.game = game;
        this.nodeRelations = relations;
        this.buildRelations();
        this.map.setView([this.game.mapCenterLat, this.game.mapCenterLng], this.game.mapZoom);
        this.map.on('click', event => this.mapClicked.emit(event));
        if (this.game != null) {
          this.createMarkers();
          this.createRelations();
          this.createNewRelations();
          //this.createContextMenu();
        }
      });
  }
  buildRelations() {
    const nodes = this.game.nodes;
    for (const relation of this.nodeRelations) {
      // Find the origin node
      const orgNode = nodes.find(n => n.id === relation.nodeId);
      const destNode = nodes.find(n => n.id === relation.childNodeId);
      orgNode.children.push(destNode);
    }
    this.nodes = this.game.nodes;
  }

  //markerRightClick(event, marker: any, component: MapDetail3Component) {
  //  let node = component.nodes.find(n => n.id === marker.id);
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
  //}
  createRelations() {

    if (this.nodes !== undefined) {
      this.nodes.forEach(node => {
        if (node.children !== undefined) {
          node.children.forEach(children => {
            const polyline = L.polyline(
              [
                [node.latitude, node.longitude],
                [children.latitude, children.longitude]
              ],
              { color: 'Blue', weight: 2 }
            );
            polyline.addTo(this.map);
            //const symbol = L.Symbol.arrowHead({ pixelSize: 10, pathOptions: { fillOpacity: 1, weight: 0 } });
            //const decorator = L.polylineDecorator(polyline,
            //  {
            //    patterns:
            //      [
            //        { offset: 0, repeat: 20, symbol: symbol }
            //      ]
            //  });
            //decorator.addTo(this.map);
            //polyline.set('node1Id', node.id);
            //polyline.set('node2Id', children.id);
            //google.maps.event.addListener(polyline,
            //  'rightclick',
            //  event => this.relationRightClick(event, polyline, this));
            //this.overlays.push(polyline);
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
    this.game.nodes.forEach(node => {
      const icon = L.icon({
        iconUrl: this.getIconForNodeType(node.nodeType)
      });
      const marker = new NodeMarker([node.latitude, node.longitude],
        { icon: icon, title: node.name, draggable: true });
      marker.node = node;
      //var marker = L.marker([node.latitude, node.longitude], { icon: icon, title: node.name, draggable: true });
      marker.addTo(this.map);
      marker.on('click', event => this.onNodeClick(event));

      //marker.addEventListener("contextmenu", event => this.markerRightClick(event.));
      //marker.addEventListener("dragend")
      this.markers.push({ marker, node });
      //google.maps.event.addListener(marker, 'rightclick', event => this.markerRightClick(event, marker, this));
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

  onNodeClick(leafletEvent: L.LeafletEvent): void { console.debug(leafletEvent); }
}
