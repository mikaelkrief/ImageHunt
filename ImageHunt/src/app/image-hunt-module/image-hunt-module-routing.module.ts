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

const routes: Routes = [
  { path: "home", component: HomeComponent },
  { path: "game", component: GameListComponent },
  { path: "game/:gameId", component: GameDetailComponent },
  { path: "action/:gameId", component: GameActionListComponent },
  { path: "action/:gameId/:teamId", component: GameActionListComponent },
  { path: "action/detail/:gameActionId", component: GameActionDetailComponent },
  { path: "team/:gameId/:teamId", component: TeamDetailComponent },
  { path: "teams/:gameId", component: TeamListComponent },
  { path: "team", component: TeamListComponent },
  { path: "admin", component: AdminListComponent },
  { path: "login", component: LoginFormComponent },
  { path: "users", component: UserRoleComponent },
  { path: "register", component: RegistrationFormComponent },
  { path: "score/:gameId", component: ScoreListComponent },
  { path: "follow/:gameId", component: TeamFollowComponent },
  { path: "passcode/:gameId", component: PasscodeListComponent },
  { path: "passcode/print/:gameId", component: PasscodePrintComponent },
  { path: "", redirectTo: "home", pathMatch: "full" },
  { path: "**", component: PageNotFoundComponent }
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class ImageHuntModuleRoutingModule { }
