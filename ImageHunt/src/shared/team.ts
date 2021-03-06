import {Player} from "./player";

export class Team {
  id: number;
  name: string;
  players: Player[];
  color: string;
  cultureInfo: string;
  bonus?: number;
  picture: any;
  code?:string;
}
