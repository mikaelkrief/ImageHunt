import {Answer} from "./answer";

export class QuestionNode {
  id: number;
  name: string;
  question:string;
  nodeType: string;
  answers:Answer[];
}
