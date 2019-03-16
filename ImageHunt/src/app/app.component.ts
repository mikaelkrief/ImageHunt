import { AlertService } from "../shared/services/alert.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent {
  constructor(public _alertService: AlertService) {}
}
