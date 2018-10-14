import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";
import {GeocoordinateComponent} from "./geocoordinate/geocoordinate.component";
import {RouterModule} from "@angular/router";
import {GoogleButtonComponent} from "./google-button/google.button.component";
import {GameActionTranslationPipe} from "./pipes/gameActionTranslationPipe";
import {UserRoleComponent} from "./user-role/user-role.component";
import { UploadImageComponent } from './upload-image/upload-image.component';

@NgModule({
  imports: [CommonModule, RouterModule],
  declarations: [GeocoordinateComponent, GoogleButtonComponent,
    GameActionTranslationPipe, UserRoleComponent,
    UploadImageComponent],
  bootstrap: [GeocoordinateComponent, GoogleButtonComponent, UserRoleComponent, UploadImageComponent],
  exports: [GeocoordinateComponent, GoogleButtonComponent, GameActionTranslationPipe, UserRoleComponent]

})
export class SharedModule {
}
