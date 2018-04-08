import {Game} from "./game";
import {Player} from "./player";
import {Node} from "./node";

export class GameAction {
  id: number;
  dateOccured: Date;
  latitude: number;
  longitude: number;
  game: Game;
  player: Player;
  action: string;
  node: Node;
  isValidated: boolean;
  delta: number;
}
