import { GeoPoint } from "../../shared/GeoPoint";
import { Node } from "../../shared/node";
import { GameService } from "../../shared/services/game.service";
import { AlertService } from "../../shared/services/alert.service";
import * as L from "leaflet";


class NodeMarker extends L.Marker {
  node: Node;
}

@Component({
  selector: "app-image-node-edit",
  templateUrl: "./image-node-edit.component.html",
  styleUrls: ["./image-node-edit.component.scss"]
})
/** imageNode-edit component*/
export class ImageNodeEditComponent implements OnInit {
  nodeMarker: NodeMarker;
  nodePosition: GeoPoint;

  @Input("node")
  set node(value: Node) {
    //this._node = value;
    this._gameService.getNodeById(value.id)
      .subscribe((node: Node) => {
        this._node = node;
        this.setMap();
      });
  }

  @Output("node")
  _nodeEmit = new EventEmitter<Node>();
  _node: Node;

  setMap() {
    this.map = L.map("location")
      .setView([this._node.latitude, this._node.longitude], 15);
    L.tileLayer("http://{s}.tile.osm.org/{z}/{x}/{y}.png",
      {
        attribution: "ImageHunt"
      }).addTo(this.map);
    const icon = L.icon({
      iconUrl: "assets/pictureNode.png",
      iconSize: [32, 32],
      iconAnchor: [16, 16]
    });

    this.nodeMarker = new NodeMarker([this._node.latitude, this._node.longitude],
      { icon: icon, title: this._node.name, draggable: true });
    this.nodeMarker.node = this._node;
    this.nodeMarker.addTo(this.map);
    this.nodeMarker.on("dragend", event => this.onNodeDragged(event));
  }

  ngOnInit(): void {

  }

  /** imageNode-edit ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService, private _alertService: AlertService) {
    this.nodePosition = new GeoPoint;
  }

  saveChanges(form) {
    this._node.name = form.form.value.name !== "" ? form.form.value.name : this._node.name;
    this._node.points = form.form.value.points !== "" ? form.form.value.points : this._node.points;
    this._gameService.updateNode(this._node)
      .subscribe(() => this._alertService.sendAlert(`Le Noeud ${this._node.name} a bien été mis à jour`,
        "success",
        5000));
  }

  map: any;

  onNodeDragged(leafletEvent: L.LeafletEvent): void {
    const newPosition = leafletEvent.target.getLatLng();
    const node = leafletEvent.target.node;
    node.latitude = newPosition.lat;
    node.longitude = newPosition.lng;
    this._nodeEmit.emit(node);

  }
}
