import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { LocalStorageModule } from "angular-2-local-storage";
import { AppComponent } from "./app.component";
import { BsDropdownModule, ModalModule, TabsModule } from "ngx-bootstrap";
import { AlertModule } from "ngx-bootstrap/alert";

import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpModule } from "@angular/http";
import { Ng2UiAuthModule, CustomConfig } from "ng2-ui-auth";
import { HomeModule } from "../home/home.module";
import { PageNotFoundModule } from "../page-not-found/page.not.found.module";
import { TeamModule } from "../team/team.module";
import { PlayerModule } from "../player/player.module";
import { TeamListComponent } from "../team/team-list/team-list.component";
import { HomeComponent } from "../home/home.component";
import { PageNotFoundComponent } from "../page-not-found/page.not.found.component";
import { NavmenuModule } from "../navmenu/navmenu.module";
import { AdminListModule } from "../admin/admin-list/admin-list.module";
import { AdminListComponent } from "../admin/admin-list/admin-list.component";
import { GameModule } from "../game/game.module";
import { GameListComponent } from "../game/game-list/game-list.component";
import { GameDetailComponent } from "../game/game-detail/game.detail.component";
import { environment } from "../environments/environment";
import { Globals } from "../shared/globals";
import { TeamDetailComponent } from "../team/team-detail/team.detail.component";
import { MapModule } from "../map/map.module";
import { MapDetail2Component } from "../map/map-detail2/map-detail2.component";
import { ContextMenuModule, InputTextModule } from "primeng/primeng";
import { NodeRelationComponent } from "../game/node-relation/node.relation.component";
import { NodeCreateComponent } from "../game/node-create/node.create.component";
import { QuestionNodeComponent } from "../game/question-node/question.node.component";
import { GameActionListComponent } from "../game/game-action-list/game-action-list.component";
import { GameActionDetailComponent } from "../game/game-action-detail/game-action-detail.component";
import localeFr from "@angular/common/locales/fr";
import { registerLocaleData } from "@angular/common";
import { SharedModule } from "../shared/shared.module";

registerLocaleData(localeFr);

export class MyAuthConfig extends CustomConfig {
  defaultHeaders = { 'Content-Type': "application/json" };
  providers = {
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
    HttpModule,
    HomeModule,
    GameModule,
    TeamModule,
    MapModule,
    PlayerModule,
    AdminListModule,
    ReactiveFormsModule,
    ContextMenuModule,
    NavmenuModule,
    PageNotFoundModule,
    SharedModule,
    InputTextModule,
    Ng2UiAuthModule.forRoot(MyAuthConfig),
    BsDropdownModule.forRoot(),
    AlertModule.forRoot(),
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    RouterModule.forRoot([
      { path: "home", component: HomeComponent },
      { path: "game", component: GameListComponent },
      { path: "game/:gameId", component: GameDetailComponent },
      { path: "action/:gameId", component: GameActionListComponent },
      { path: "action/detail/:gameActionId", component: GameActionDetailComponent },
      { path: "team/:teamId", component: TeamDetailComponent },
      { path: "team", component: TeamListComponent },
      { path: "map", component: MapDetail2Component },
      { path: "admin", component: AdminListComponent },
      { path: "", redirectTo: "home", pathMatch: "full" },
      { path: "**", component: PageNotFoundComponent }
    ])
  ],
  providers: [Globals],
  entryComponents: [NodeCreateComponent, NodeRelationComponent, QuestionNodeComponent]

})
export class AppModule {
}
