import { Player } from "../../shared/player";

@Component({
  selector: "player-create",
  templateUrl: "./player-create.component.html",
  styleUrls: ["./player-create.component.scss"]
})
/** player-create component*/
export class PlayerCreateComponent implements OnInit {
  ngOnInit(): void {

  }

  playerCreateForm: FormGroup;

  @Output()
  playerCreated = new EventEmitter<Player>();

  /** player-create ctor */
  constructor(public bsModalRef: BsModalRef) {

  }

  hasFormErrors() {
    return !this.playerCreateForm.valid;
  }

  fieldErrors(field: string) {
    const controlState = this.playerCreateForm.controls[field];
    return (controlState.dirty && controlState.errors) ? controlState.errors : null;
  }

  createPlayer() {

    const player: Player = {
      id: 0,
      name: this.playerGroup.value.name,
      chatLogin: this.playerGroup.value.chatLogin,
      startDate: null
    };
    this.playerCreated.emit(player);
  }

  playerGroup = new FormGroup({
    name: new FormControl(""),
    chatLogin: new FormControl("")
  });

}
