export class QuestionNodeAnswerRelation {
  id: number;
  childrenId: number;
  answerId: number;
  constructor(nodeId: number, childrenId: number, answerId: number) {
    this.id = nodeId;
    this.childrenId = childrenId;
    this.answerId = answerId;
  }
}
