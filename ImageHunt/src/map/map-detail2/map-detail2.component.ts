import { Component, Input, OnInit, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import {Node} from '../../shared/node';
import {NodeRelation} from '../../shared/NodeRelation';
import {GeoPoint} from '../../shared/GeoPoint';
import { GameService } from '../../shared/services/game.service';
import { Game } from '../../shared/game';
import { ActivatedRoute } from '@angular/router';
import {NodeClicked} from "../../shared/NodeClicked";
import {RelationClicked} from "../../shared/RelationClicked";

@Component({
    selector: 'map-detail2',
    templateUrl: './map-detail2.component.html',
    styleUrls: ['./map-detail2.component.scss']
})
/** map-detail2 component*/
export class MapDetail2Component implements OnInit, OnChanges {
  options: any;
  overlays: any[];
  game: Game;
  nodeRelations: NodeRelation[];
  isFirstClick: boolean = true;

  @Input() gameId: number;
  @Input() nodes: Node[];
  @Input() nodesRelation: NodeRelation[];
  @Input() newNodesRelation: GeoPoint[];
  @Input() nodeMode: string;
  @Input() filterNode: string[];
  @Output() mapClicked = new EventEmitter();
  @Output() nodeClicked = new EventEmitter<NodeClicked>();
  @Output() nodeRightClicked = new EventEmitter<NodeClicked>();
  @Output() relationRightClicked = new EventEmitter<RelationClicked>();
  @Output() newRelation = new EventEmitter<NodeRelation>();
  @Output() zoomChange = new EventEmitter<number>();

    /** map-detail2 ctor */
  constructor(private _gameService: GameService) {
    this.options = {
      center: { lat: 0, lng: 0 },
      zoom: 12
    };

  }
  ngOnInit(): void {
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.updateMap();
  }
  map: google.maps.Map;
  setMap(event) {
    this.map = event.map;
  }
  updateMap() {

    this._gameService.getGameById(this.gameId)
      .subscribe(res => {
        this.game = res;
        this.options = {
          center: { lat: this.game.mapCenterLat, lng: this.game.mapCenterLng },
          zoom: this.game.mapZoom
        };
        this.map.setCenter(this.options.center);
        this.map.setZoom(this.options.zoom);
        if (this.game != null) {
          this.overlays = [];
          const center = new google.maps.Marker({
            position: this.options.center,
            title:'center'
          });
          google.maps.event.addListener(center, 'rightclick', event => this.markerRightClick(event, center, this));
          this.overlays.push(center);
          this.createMarkers();
          this.createRelations();
          this.createNewRelations();
          this.createContextMenu();
        }
      });
  }


  createMarkers() {
  this.game.nodes.forEach(node => {
    const marker = new google.maps.Marker({
      position: { lat: node.latitude, lng: node.longitude },
      title: node.name,
      icon: this.getIconForNodeType(node.nodeType),
    });
    marker.set('id', node.id);
    google.maps.event.addListener(marker, 'rightclick', event=>this.markerRightClick(event, marker, this));
    this.overlays.push(marker);
  });
}
createRelations() {
  const arrowSymbol = {
    path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
  };
  if (this.nodes !== undefined) {
    this.nodes.forEach(node => {
      if (node.children !== undefined) {
        node.children.forEach(children => {
          const polyline = new google.maps.Polyline(
            {
              strokeColor: 'Blue',
              strokeWeight: 2,
              path: [
                { lat: node.latitude, lng: node.longitude },
                { lat: children.latitude, lng: children.longitude }
              ],
              icons: [{ icon: arrowSymbol, offset: '50%' }]
            });
          polyline.set('node1Id', node.id);
          polyline.set('node2Id', children.id);
          this.overlays.push(polyline);
          google.maps.event.addListener(polyline,
            'rightclick',
            event => this.relationRightClick(event, polyline, this));
        });
      }
    });
  }
}
createNewRelations() {
  const arrowSymbol = {
    path: google.maps.SymbolPath.FORWARD_OPEN_ARROW
  };
  if (this.newRelation !== undefined) {
    this.newRelation.forEach(relation => {
      const firstNode = this.nodes.find(n => n.id === relation.nodeId);
      const secondNode = this.nodes.find(n => n.id === relation.childNodeId);
      const polyline = new google.maps.Polyline(
        {
          strokeColor: 'Red',
          strokeWeight: 2,
          path: [
            { lat: firstNode.latitude, lng: firstNode.longitude },
            { lat: secondNode.latitude, lng: secondNode.longitude }
          ],
          icons: [{ icon: arrowSymbol, offset: '50%' }]
        });
      this.overlays.push(polyline);
    });
  }
}


  onMapClick(event) {
    this.mapClicked.emit(event);
  }
  isFirstClicked: boolean = true;
  firstNode: Node;
  secondNode: Node;
  resetNodeClick() {
    this.firstNode = null;
    this.isFirstClick = true;
  }
  onOverlayClick(event) {
    const isMarker = event.overlay.getTitle != undefined;
    if (isMarker) {
      let node = this.nodes.find(n => n.id === event.overlay.id);
      let nClicked: NodeClicked;
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
      
      this.nodeClicked.emit(nClicked);
    }
  }
  markerRightClick(event, marker: any, component: MapDetail2Component) {
    let node = component.nodes.find(n => n.id === marker.id);
    const nClicked = new NodeClicked(node, 0, event.Ia);
    this.nodeRightClicked.emit(nClicked);
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

  createContextMenu() {
  }

  relationRightClick(event, polyline: google.maps.Polyline, component: this) {

  }
}
