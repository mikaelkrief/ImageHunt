import { Admin } from "../../shared/admin";

@Component({
  selector: "admin-create",
  templateUrl: "./admin-create.component.html",
  styleUrls: ["./admin-create.component.scss"]
})
/** admin-create component*/
export class AdminCreateComponent {
  @Output()
  adminCreated = new EventEmitter<Admin>();

  /** admin-create ctor */
  constructor(public bsModalRef: BsModalRef) {

  }

  createAdmin() {
    const admin: Admin = {
      id: 0,
      name: this.adminGroup.value.name,
      email: this.adminGroup.value.email,
      role: this.adminGroup.value.role
    };
    this.adminCreated.emit(admin);
    this.adminGroup.reset();
  }

  adminGroup = new FormGroup({
    name: new FormControl(""),
    email: new FormControl(""),
    role: new FormControl(0)
  });
}
