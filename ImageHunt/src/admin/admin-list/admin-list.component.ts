import {AdminService} from "../../shared/services/admin.service";
import {Admin} from "../../shared/admin";
import { ConfirmationService } from "primeng/api";
import { Component, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { BsModalService } from "ngx-bootstrap";
import { AdminCreateComponent } from "../admin-create/admin-create.component";
import { GameAssignComponent } from "../game-assign/game-assign.component";
import { GameService } from "services/game.service";
import { forkJoin } from "rxjs";

@Component({
  selector: 'admin-list',
  templateUrl: './admin-list.component.html',
  styleUrls: ['./admin-list.component.scss']
})
/** admin component*/
export class AdminListComponent implements OnInit {
  admins: Admin[];
  /** admin ctor */
  constructor(private _adminService: AdminService,
    private _confirmationService: ConfirmationService,
    private _gameService: GameService,
    private _modalService: BsModalService) {

  }

  /** Called by Angular after admin component initialized */
  ngOnInit(): void {
    this.getAdmins();
  }
  public getAdmins() {
    forkJoin(
      this._adminService.getAllAdmins(),
        this._gameService.getAllGame())
      .subscribe((res: any[]) => {
          this.admins = res[0];
        this.games = res[1];
          this.admins.map(a => a.games = this.games.filter(g => a.gameIds.includes(g.id)));
        },
           err => console.error(err.status));
  }
  deleteAdmin(adminId: number) {
    this._confirmationService.confirm({
      message: "Etes-vous sur de vouloir supprimer cet administrateur ?",
      accept: () => this._adminService.deleteAdmin(adminId)
        .subscribe(null, null, () => this.getAdmins())
    });

  }
  createUser() {
    this.modalRef = this._modalService.show(AdminCreateComponent, { ignoreBackdropClick: true });
    this.modalRef.content.adminCreated.subscribe((admin: Admin) => {
      this._adminService.createAdmin(admin)
        .subscribe(() => {
          this.getAdmins();
        });
    });
  }
  assignGame(admin: Admin) {
    this.modalRef = this._modalService.show(GameAssignComponent, { ignoreBackdropClick: true, initialState: {admin} });
    this.modalRef.content.admin
  }
  modalRef;
  games;
}
