import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from "../../home/home.component";
import { GameListComponent } from "../../game/game-list/game-list.component";
import { GameDetailComponent } from "../../game/game-detail/game.detail.component";
import { GameActionListComponent } from "../../game/game-action-list/game-action-list.component";
import { GameActionDetailComponent } from "../../game/game-action-detail/game-action-detail.component";
import { TeamDetailComponent } from "../../team/team-detail/team.detail.component";
import { TeamListComponent } from "../../team/team-list/team-list.component";
import { AdminListComponent } from "../../admin/admin-list/admin-list.component";
import { LoginFormComponent } from "../../account/login-form/login-form.component";
import { RegistrationFormComponent } from "../../account/registration-form/registration-form.component";
import { ScoreListComponent } from "../../score/score-list/score-list.component";
import { TeamFollowComponent } from "../../team/team-follow/team-follow.component";
import { PasscodeListComponent } from "../../game/passcode-list/passcode-list.component";
import { PasscodePrintComponent } from "../../game/passcode-print/passcode-print.component";
import { PageNotFoundComponent } from "../../page-not-found/page.not.found.component";
import { UserRoleComponent } from "../../account/user-role/user-role.component";
import { AuthGuard } from '../../shared/auth-guard';
import { GameTeamsComponent } from '../../team/game-teams/game-teams.component';
import { GameAvailableComponent } from '../../game/game-available/game-available.component';
import { GameValidationComponent } from '../../game/game-validation/game-validation.component';

const routes: Routes = [
  { path: "home", component: HomeComponent },
  { path: "game", component: GameListComponent, canActivate: [AuthGuard], data: { roles: ["Admin", "GameMaster"] } },
  { path: "game/:gameId", component: GameDetailComponent, canActivate: [AuthGuard], data: { roles: ["Admin", "GameMaster"] } },
  { path: "action/:gameId", component: GameActionListComponent, canActivate: [AuthGuard], data: { roles: ["Admin", "GameMaster", "Validator"] } },
  { path: "action/:gameId/:teamId", component: GameActionListComponent, canActivate: [AuthGuard], data: { roles: ["Admin", "GameMaster", "Validator"] } },
  { path: "action/detail/:gameActionId", component: GameActionDetailComponent, canActivate: [AuthGuard], data: { roles: ["Admin", "GameMaster", "Validator"] } },
  { path: "team/:gameId/:teamId", component: TeamDetailComponent },
  { path: "teams/:gameId", component: TeamListComponent },
  { path: "team", component: TeamListComponent },
  { path: "gameavailable", component: GameAvailableComponent},
  { path: "gamevalidation", component: GameValidationComponent},
  { path: "gameteam/:gameCode", component: GameTeamsComponent, canActivate: [AuthGuard], data: { roles: ["Admin", "TeamLeader", "Player"] } },
  { path: "admin", component: AdminListComponent, canActivate: [AuthGuard], data: { roles: ["Admin"] } },
  { path: "login", component: LoginFormComponent },
  { path: "users", component: UserRoleComponent, canActivate: [AuthGuard], data: {roles:["Admin"]} },
  { path: "register", component: RegistrationFormComponent },
  { path: "score/:gameId", component: ScoreListComponent },
  { path: "follow/:gameId", component: TeamFollowComponent },
  { path: "passcode/:gameId", component: PasscodeListComponent, canActivate: [AuthGuard], data: { roles: ["Admin", "GameMaster"] } },
  { path: "passcode/print/:gameId", component: PasscodePrintComponent, canActivate: [AuthGuard], data: { roles: ["Admin", "GameMaster"] } },
  { path: "", redirectTo: "home", pathMatch: "full" },
  { path: "**", component: PageNotFoundComponent }
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class ImageHuntModuleRoutingModule { }
