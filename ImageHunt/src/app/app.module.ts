import { BrowserModule } from "@angular/platform-browser";
import { LocalStorageModule } from "angular-2-local-storage";
import { AppComponent } from "./app.component";
import { AlertModule } from "ngx-bootstrap/alert";
import "@angular/common";

import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
//import { Ng2UiAuthModule } from "ng2-ui-auth";
import { HomeModule } from "../home/home.module";
import { PageNotFoundModule } from "../page-not-found/page.not.found.module";
import { TeamModule } from "../team/team.module";
import { PlayerModule } from "../player/player.module";
import { NavmenuModule } from "../navmenu/navmenu.module";
import { AdminModule } from "../admin/admin.module";
import { GameModule } from "../game/game.module";
import { ScoreModule } from "../score/score.module";
import { environment } from "../environments/environment";
import { MapModule } from "../map/map.module";
import { ContextMenuModule, InputTextModule, ListboxModule } from "primeng/primeng";
import { NodeRelationComponent } from "../game/node-relation/node.relation.component";
import { NodeCreateComponent } from "../game/node-create/node.create.component";
import { QuestionNodeComponent } from "../game/question-node/question.node.component";
import localeFr from "@angular/common/locales/fr";
import { registerLocaleData } from "@angular/common";
import { SharedModule } from "../shared/shared.module";
import { ImageNodeEditComponent } from "../game/image-node-edit/image-node-edit.component";
import { IPartialConfigOptions, IProviders } from "ng2-ui-auth/lib/config-interfaces";
import { PasscodeCreateComponent } from "../game/passcode-create/passcode-create.component";
import { TeamCreateComponent } from "../team/team-create/team-create.component";
import { MomentModule } from "angular2-moment";
import { GameCreateComponent } from "../game/game-create/game.create.component";
import { PlayerCreateComponent } from "../team/player-create/player-create.component";
import { AccountModule } from "../account/account.module";
import { RegistrationFormComponent } from "../account/registration-form/registration-form.component";
import { JwtModule } from "@auth0/angular-jwt";
import { ImageHuntModuleRoutingModule } from "./image-hunt-module/image-hunt-module-routing.module";
import { BatchNodeComponent } from "../game/batch-node/batch-node.component";
registerLocaleData(localeFr);

export function tokenGetter() {
  return localStorage.getItem("authToken");
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
    MomentModule,
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
  entryComponents: [
    NodeCreateComponent, NodeRelationComponent, QuestionNodeComponent,
    ImageNodeEditComponent, PasscodeCreateComponent, TeamCreateComponent, GameCreateComponent, PlayerCreateComponent,
    RegistrationFormComponent, BatchNodeComponent
  ],
  providers: [BsModalService]

})
export class AppModule {
}
