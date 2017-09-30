import { Node } from './node';
export class Game {
  id: number;
  isActive: boolean;
  name: string;
  startDate: Date;
  mapCenterLat: number;
  mapCenterLng: number;
  mapZoom:number;
  nodes: Node[];
}
