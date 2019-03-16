export class NodeResponse {
  id: number;
  name: string;
  nodeType: string;
  latitude: number;
  longitude: number;
  points: number;
  password: string;
  childNodeIds: number[];
  action: string;
}
