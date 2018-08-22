import { Team } from "./team";
import { Game } from "./game";

export class TeamPosition {
  team: Team;
  dateOccured: Date;
  position: L.LatLng;
}
