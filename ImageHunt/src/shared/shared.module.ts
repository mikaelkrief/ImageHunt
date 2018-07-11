import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {GeocoordinateComponent} from "./geocoordinate/geocoordinate.component";
import {RouterModule} from "@angular/router";
import { GoogleButtonComponent } from "./google-button/google.button.component";
import { GithubButtonComponent } from "./github-button/github.button.component";
import {GameActionTranslationPipe} from "./pipes/gameActionTranslationPipe";
import {UserRoleComponent} from "./user-role/user-role.component";

@NgModule({
  imports: [CommonModule, RouterModule],
  declarations: [GeocoordinateComponent, GoogleButtonComponent, GithubButtonComponent, GameActionTranslationPipe, UserRoleComponent],
  bootstrap: [GeocoordinateComponent, GoogleButtonComponent, GithubButtonComponent, UserRoleComponent],
  exports: [GeocoordinateComponent, GoogleButtonComponent, GithubButtonComponent, GameActionTranslationPipe, UserRoleComponent]

})
export class SharedModule {
}
