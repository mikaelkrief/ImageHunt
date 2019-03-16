import { NodeRequest } from "../../shared/nodeRequest";

@Component({
  selector: "node-create",
  templateUrl: "./node.create.component.html",
  styleUrls: ["./node.create.component.scss"]
})
/** node-create component*/
export class NodeCreateComponent implements OnInit {
  latitude: number;
  longitude: number;
  correctAnswer: number;
  choices: string[];
  @Output()
  newNode = new EventEmitter<NodeRequest>();
  nodeTypes = [
    { color: "red", value: "FirstNode", label: "Start", icon: "fa fa-flag" },
    { color: "cadetblue", value: "WaypointNode", label: "Waypoint", icon: "fas fa-map-marker-alt" },
    { color: "green", value: "TimerNode", label: "Timer", icon: "fas fa-clock" },
    { color: "purple", value: "ObjectNode", label: "Action", icon: "fas fa-running" },
    { color: "violet", value: "HiddenNode", label: "Hidden", icon: "fas fa-mask" },
    { color: "green", value: "ChoiceNode", label: "Choix", icon: "fas fa-list-ul" },
    { color: "blue", value: "QuestionNode", label: "Question", icon: "fas fa-question" },
    { color: "blue", value: "PictureNode", label: "Picture", icon: "fas fa-camera" },
    { color: "purple", value: "BonusNode", label: "Bonus", icon: "fas fa-gift" },
    { color: "green", value: "LastNode", label: "End", icon: "fas fa-flag-checkered" },
  ];

  /** node-create ctor */
  constructor(public bsModalRef: BsModalRef) {}

  /** Called by Angular after node-create component initialized */
  ngOnInit(): void {}

  createNode(form: NgForm) {
    const node: NodeRequest = {
      nodeType: form.value.nodeType,
      name: form.value.name,
      latitude: this.latitude,
      longitude: this.longitude,
      duration: form.value.duration === "" ? 0 : form.value.duration,
      action: form.value.action,
      question: form.value.question,
      answer: form.value.answer,
      points: form.value.nbPoints === "" ? 0 : form.value.nbPoints,
      choices: null,
      password: form.value.password,
      hint: form.value.hint,
      bonus: form.value.bonustype === "" ? 0 : form.value.bonustype,
      location: form.value.location

    };
    // Add answers
    if (form.value.nodeType === "ChoiceNode") {
      node.choices = new Array();
      node.question = form.value.choiceQuestion;
      for (let i = 0; i < this.choices.length; i++) {
        node.choices.push({ response: this.choices[i], correct: i === +this.correctAnswer });
      }
    }
    this.newNode.emit(node);
  }

  addChoice(newChoice: NgForm) {
    if (this.choices == null) {
      this.choices = [];
    }
    this.choices.push(newChoice.value.choice);
    newChoice.reset();
  }
}
