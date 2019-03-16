export class QuestionNodeAnswerRelation {
  nodeId: number;
  childrenId: number;
  answerId: number;

  constructor(nodeId: number, childrenId: number, answerId: number) {
    this.nodeId = nodeId;
    this.childrenId = childrenId;
    this.answerId = answerId;
  }
}
