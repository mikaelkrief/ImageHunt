import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'points',
    templateUrl: './points.component.html',
    styleUrls: ['./points.component.scss']
})
/** points component*/
export class PointsComponent implements OnInit {
  @Input()
  get points(): number {
    return this._points;
  }
  @Output() pointsChange = new EventEmitter<number>();
  set points(val: number) {
    this._points = val;
    this.pointsChange.emit(this.points);
  } 

    ngOnInit(): void {
        
    }
    /** points ctor */
    constructor() {
      this._editMode = false;
  }
  editToggle() {
    this._editMode = !this._editMode;
  }
  validate() {
    this.pointsChange.emit(this.points);
    this._editMode = !this._editMode;
  }
  cancel() {
    this.points = 0;
    this._editMode = false;
  }
  _editMode: boolean;
  _points: number;
}
