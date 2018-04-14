export class NodeRequest {
  nodeType: string;
  name: string;
  latitude: number;
  longitude: number;
  duration: number;
  action: string;
  question: string;
  answers: {response:string, correct:boolean}[];
}
