import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/services/user.service';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})
export class RegistrationFormComponent implements OnInit {

  constructor(private userService: UserService) { }

  ngOnInit() {
  }
  register(form) {
    let user = form.form.value;
    this.userService.register(user.email, user.password, user.userName, user.telegram)
      .subscribe(res => {});
  }
}
