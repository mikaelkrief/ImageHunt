import { Answer } from "./answer";

export class QuestionNode {
  nodeId: number;
  name: string;
  question: string;
  nodeType: string;
  answers: Answer[];
}
