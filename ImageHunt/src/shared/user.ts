import { Game } from "./game";

export interface User {
  userName: string;
  email: string;
  telegram: string;
  role: string;
  games?: Game[];
  appUserId?: number;
};
