import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'user-role',
    templateUrl: './user-role.component.html',
    styleUrls: ['./user-role.component.scss']
})
/** user-role component*/
export class UserRoleComponent implements OnInit{
  ngOnInit(): void {
    switch (this.role) {
    case 0:
        this.label = "Administrateur";
    break;
    case 1:
      this.label = "Validateur";
      break;
    case 2:
      this.label = "Créateur de carte";
      break;
    case 3:
      this.label = "Lecteur";
      break;
    }
  } /** user-role ctor */
  @Input() role: any;

    constructor() {

    }

  label: string;
}
