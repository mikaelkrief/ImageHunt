import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'dual-list',
    templateUrl: './dual-list.component.html',
    styleUrls: ['./dual-list.component.scss']
})
/** dual-list component*/
export class DualListComponent {
  @Input() addLabel: string;
  @Input() removeLabel: string;
  @Input() allLabel: string;
  @Input() noneLabel: string;
  @Input() key: string;
  @Input() display: string;
  @Input() source: any[];
  @Input() destination: any[];
  @Output() destinationChange: EventEmitter<any[]> = new EventEmitter<any[]>();
  sourceSelected: any[]=[];
  destinationSelected: any[]=[];
  
    /** dual-list ctor */
    constructor() {
  }
  noneSourceSelected() {
    this.sourceSelected = [];
  }
  noneDestinationSelected() {
    this.destinationSelected = [];
  }
  addSourceSelectedToDestination() {
    this.sourceSelected.map(ss => this.destination = [...this.destination, ss]);
    this.source = this.source.filter(s => !this.sourceSelected.includes(s));
    this.destinationChange.emit(this.destination);
  }
  removeDestinationSelectedToSource() {
    this.destinationSelected.map(ss => this.source = [...this.source, ss]);
    this.destination = this.destination.filter(s => !this.destinationSelected.includes(s));
    this.destinationChange.emit(this.destination);
  }
}
