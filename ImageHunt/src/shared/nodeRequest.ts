export class NodeRequest {
  nodeType: string;
  name: string;
  latitude: number;
  longitude: number;
  duration: number;
  action: string;
  question: string;
  points:number;
  answers: { response: string, correct: boolean }[];
  password: string;
  hint: string;
}
