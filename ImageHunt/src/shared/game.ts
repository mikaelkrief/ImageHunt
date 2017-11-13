import { Node } from './node';
import {Team} from "./team";

export class Game {
  id: number;
  isActive: boolean;
  name: string;
  startDate: Date;
  mapCenterLat: number;
  mapCenterLng: number;
  mapZoom:number;
  nodes: Node[];
  teams: Team[];
}
