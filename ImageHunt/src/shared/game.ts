import { Node } from './node';
import {Team} from "./team";
import { NodeResponse } from './nodeResponse';

export class Game {
  id: number;
  isActive: boolean;
  isPublic: boolean;
  name: string;
  startDate?: Date;
  mapCenterLat?: number;
  mapCenterLng?: number;
  mapZoom?:number;
  nodes?: NodeResponse[];
  teams?: Team[];
  pictureId?: number;
  description?: string;
  code?: string;
}
