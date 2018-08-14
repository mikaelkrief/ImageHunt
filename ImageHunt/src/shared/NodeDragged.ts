import { Node } from './node';

export class NodeDragged {
  constructor(public node: Node, public newPosition:L.LatLng) { }

}
