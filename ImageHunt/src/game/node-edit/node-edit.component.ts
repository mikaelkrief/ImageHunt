import { Component, OnInit } from '@angular/core';
import { Node } from 'shared/Node';

@Component({
    selector: 'node-edit',
    templateUrl: './node-edit.component.html',
    styleUrls: ['./node-edit.component.scss']
})
/** node-edit component*/
export class NodeEditComponent implements OnInit {
    ngOnInit(): void {
      this.modeTitle = this.createMode ? "Create" : "Edit";
  }
  public node: Node;
  public createMode: boolean;
  modeTitle: string;
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

    /** node-edit ctor */
    constructor() {
      this.modeTitle = "Create";
      this.node = new Node();
    }
}
