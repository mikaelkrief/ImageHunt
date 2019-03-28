import { Node } from './node';
import {Team} from "./team";

export class Game {
  id: number;
  isActive: boolean;
  isPublic: boolean;
  name: string;
  startDate?: Date;
  mapCenterLat?: number;
  mapCenterLng?: number;
  mapZoom?:number;
  nodes?: Node[];
  teams?: Team[];
  pictureId?: number;
  description?: string;
  code?: string;
}
