import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import {Node as Node1} from "../../shared/node";
import {NodeRelation} from "../../shared/NodeRelation";
import {GeoPoint} from "../../shared/GeoPoint";

@Component({
    selector: 'map-detail2',
    templateUrl: './map-detail2.component.html',
    styleUrls: ['./map-detail2.component.scss']
})
/** map-detail2 component*/
export class MapDetail2Component implements OnInit {
  ngOnInit(): void {
    this.options = {
      center: { lat: this.CenterLat, lng: this.CenterLng },
      zoom: 12
    };
  }

  @Input() public CenterLat: number;
  @Input() public CenterLng: number;
  @Input() zoom: number;
  @Input() gameId: number;
  @Input() nodes: Node1[];
  @Input() nodesRelation: NodeRelation[];
  @Input() newNodesRelation: GeoPoint[];
  @Input() nodeMode: string;
  @Input() filterNode: string[];
  @Output() mapClicked = new EventEmitter();
  @Output() nodeClicked = new EventEmitter<Node1>();
  @Output() newRelation = new EventEmitter<NodeRelation>();
  @Output() zoomChange = new EventEmitter<number>();

    /** map-detail2 ctor */
    constructor() {

    }

  options: any;
}
