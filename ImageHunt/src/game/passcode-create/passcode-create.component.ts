import { GameService } from "services/game.service";
import { Passcode } from "../../shared/Passcode";

@Component({
  selector: "passcode-create",
  templateUrl: "./passcode-create.component.html",
  styleUrls: ["./passcode-create.component.scss"]
})
/** passcode-create component*/
export class PasscodeCreateComponent {
  /** passcode-create ctor */
  constructor(public bsModalRef: BsModalRef, private _gameService: GameService) {

  }

  gameId: number;
  @Output()
  newPasscode = new EventEmitter<Passcode>();
  pass: string;
  nbRedeem = -1;
  points = 10;

  createPasscode() {
    const passcode: Passcode = { id: 0, pass: this.pass, nbRedeem: this.nbRedeem, points: this.points, qrCode: "" };
    this.newPasscode.emit(passcode);
  }

  generatePassword() {
    this.pass = "";
    for (let i = 0; i < 20; i++) {
      this.pass += Math.random().toString(36).replace(/[^a-z]+/g, "").substr(0, 1);
    }
    this.pass = this.pass.toUpperCase();
  }

}
