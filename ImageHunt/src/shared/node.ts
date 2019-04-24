import { Answer } from "./answer";

export class Node {
  id: number;
  image: any;
  latitude: number;
  longitude: number;
  name: string;
  children: Node[];
  nodeType: string;
  duration: number;
  action?: string;
  points: number;
  delta: number;
  password?: string;
  delay?: number;
  bonus?: number;
  hint?: string;
  question?: string;
  answer?: string;
  choices?: Answer[];
}
