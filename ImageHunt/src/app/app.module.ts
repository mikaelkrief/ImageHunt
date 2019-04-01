import { BrowserModule } from "@angular/platform-browser";
import { RouterModule, Routes } from "@angular/router";
import { LocalStorageModule } from "angular-2-local-storage";
import { AppComponent } from "./app.component";
import { BsDropdownModule, ModalModule, TabsModule, ButtonsModule, TooltipModule, AccordionModule, BsModalService  } from "ngx-bootstrap";
import { AlertModule } from "ngx-bootstrap/alert";
import '@angular/common';

import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
//import { Ng2UiAuthModule } from "ng2-ui-auth";
import { HomeModule } from "../home/home.module";
import { PageNotFoundModule } from "../page-not-found/page.not.found.module";
import { TeamModule } from "../team/team.module";
import { PlayerModule } from "../player/player.module";
import { TeamListComponent } from "../team/team-list/team-list.component";
import { HomeComponent } from "../home/home.component";
import { PageNotFoundComponent } from "../page-not-found/page.not.found.component";
import { NavmenuModule } from "../navmenu/navmenu.module";
import { AdminModule } from "../admin/admin.module";
import { AdminListComponent } from "../admin/admin-list/admin-list.component";
import { GameModule } from "../game/game.module";
import { ScoreModule } from "../score/score.module";
import { GameListComponent } from "../game/game-list/game-list.component";
import { GameDetailComponent } from "../game/game-detail/game.detail.component";
import { environment } from "../environments/environment";
import { Globals } from "../shared/globals";
import { TeamDetailComponent } from "../team/team-detail/team.detail.component";
import { MapModule } from "../map/map.module";
import { ContextMenuModule, InputTextModule, ListboxModule } from "primeng/primeng";
import { NodeRelationComponent } from "../game/node-relation/node.relation.component";
import { NodeCreateComponent } from "../game/node-create/node.create.component";
import { QuestionNodeComponent } from "../game/question-node/question.node.component";
import { GameActionListComponent } from "../game/game-action-list/game-action-list.component";
import { GameActionDetailComponent } from "../game/game-action-detail/game-action-detail.component";
import localeFr from "@angular/common/locales/fr";
import { registerLocaleData } from "@angular/common";
import { SharedModule } from "../shared/shared.module";
import { QRCodeModule } from "angular2-qrcode";
import { ScoreListComponent } from '../score/score-list/score-list.component';
import { ImageNodeEditComponent } from "../game/image-node-edit/image-node-edit.component";
import { TeamFollowComponent } from "../team/team-follow/team-follow.component";
import { IPartialConfigOptions, IProviders } from "ng2-ui-auth/lib/config-interfaces";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { PasscodeListComponent } from "../game/passcode-list/passcode-list.component";
import { PasscodeCreateComponent } from "../game/passcode-create/passcode-create.component";
import { PasscodePrintComponent } from "../game/passcode-print/passcode-print.component";
import { TeamCreateComponent } from "../team/team-create/team-create.component";
import { GameCreateComponent } from "../game/game-create/game.create.component";
import { PlayerCreateComponent } from "../team/player-create/player-create.component";
import { LoginFormComponent } from "../account/login-form/login-form.component";
import { AccountModule } from "../account/account.module";
import { RegistrationFormComponent } from "../account/registration-form/registration-form.component";
import { UserRoleComponent } from "../account/user-role/user-role.component";
import { JwtModule } from '@auth0/angular-jwt';
import { ImageHuntModuleRoutingModule } from "./image-hunt-module/image-hunt-module-routing.module";
import { BatchNodeComponent } from "../game/batch-node/batch-node.component";
import { NodeEditComponent } from "../game/node-edit/node-edit.component";
registerLocaleData(localeFr);
export function tokenGetter() {
  return localStorage.getItem('authToken');
}

export class MyAuthConfig implements IPartialConfigOptions {
  defaultHeaders = { 'Content-Type': "application/json" };
  providers: IProviders = {
    google: { clientId: environment.GOOGLE_CLIENT_ID },
  };
  tokenName = "accessToken";
  tokenPrefix = "";
  baseUrl = environment.API_ENDPOINT;
}



@NgModule({
  declarations: [
    AppComponent
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    LocalStorageModule.withConfig({
      prefix: "Img-Hunt",
      storageType: "localStorage"
    }),
    FormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
      }
    }),
    HomeModule,
    GameModule,
    TeamModule,
    AccountModule,
    ScoreModule,
    MapModule,
    PlayerModule,
    AdminModule,
    ReactiveFormsModule,
    ContextMenuModule,
    NavmenuModule,
    PageNotFoundModule,
    SharedModule,
    InputTextModule,
    ListboxModule,
    BsDropdownModule.forRoot(),
    TooltipModule.forRoot(),
    AlertModule.forRoot(),
    TabsModule.forRoot(),
    ButtonsModule.forRoot(),
    ModalModule.forRoot(),
    AccordionModule.forRoot(),
    ImageHuntModuleRoutingModule,

  ],
  entryComponents: [NodeCreateComponent, NodeEditComponent, NodeRelationComponent, QuestionNodeComponent,
    ImageNodeEditComponent, PasscodeCreateComponent, TeamCreateComponent, GameCreateComponent, PlayerCreateComponent, RegistrationFormComponent, BatchNodeComponent],
  providers: [BsModalService]

})
export class AppModule {
}
