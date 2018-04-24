import { Component, Input, OnInit, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import {Node} from '../../shared/node';
import {NodeRelation} from '../../shared/NodeRelation';
import {GeoPoint} from '../../shared/GeoPoint';
import { GameService } from '../../shared/services/game.service';
import { Game } from '../../shared/game';
import { ActivatedRoute } from '@angular/router';

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

  @Input() gameId: number;
  @Input() nodes: Node[];
  @Input() nodesRelation: NodeRelation[];
  @Input() newNodesRelation: GeoPoint[];
  @Input() nodeMode: string;
  @Input() filterNode: string[];
  @Output() mapClicked = new EventEmitter();
  @Output() nodeClicked = new EventEmitter<Node>();
  @Output() newRelation = new EventEmitter<NodeRelation>();
  @Output() zoomChange = new EventEmitter<number>();

    /** map-detail2 ctor */
  constructor(private _gameService: GameService) {
    this.options = {
      center: { lat: 48.848253151521625, lng: 2.336956914514303 },
      zoom: 12
    };

  }
  ngOnInit(): void {
    //this.updateMap();
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.updateMap();
  }

  updateMap() {

    this._gameService.getGameById(this.gameId)
      .subscribe(res => {
        this.game = res;
        this.options = {
          center: { lat: this.game.mapCenterLat, lng: this.game.mapCenterLng },
          zoom: this.game.mapZoom
        };
        if (this.game != null) {

          this.overlays = [];
          this.createMarkers();
          this.createRelations();
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
          this.overlays.push(polyline);
        });
      }
    });
  }
}

  onMapClick(event) {
    this.mapClicked.emit(event);
  }

  onOverlayClick(event) {
    const isMarker = event.overlay.getTitle != undefined;
    if (isMarker) {
      const node = this.nodes.find(n => n.id === event.overlay.id);
      
      this.nodeClicked.emit(node);
    }
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

}
