import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'map',
    templateUrl: './map.component.html',
    styleUrls: ['./map.component.scss']
})
/** map component*/
export class MapComponent implements OnInit
{
  title: string = 'My first AGM project';
  lat: number = 51.678418;
  lng: number = 7.809007;
    /** map ctor */
    constructor() { }

    /** Called by Angular after map component initialized */
    ngOnInit(): void { }
}
