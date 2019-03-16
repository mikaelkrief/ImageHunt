import { Game } from "./game";

export class Admin {
  id: number;
  name: string;
  email: string;
  games?: Game[];
  gameIds?: number[];
  role: any;
}
