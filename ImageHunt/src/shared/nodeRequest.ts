export class NodeRequest {
  nodeType: string;
  name: string;
  latitude: number;
  longitude: number;
  duration: number;
  action: string;
  question: string;
  answer: string;
  points: number;
  choices: { response: string, correct: boolean }[];
  password: string;
  hint: string;
  bonus: number;
  location: string;
}
