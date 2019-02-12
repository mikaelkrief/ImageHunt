import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { GeocoordinateComponent } from "./geocoordinate/geocoordinate.component";
import { RouterModule } from "@angular/router";
import { GoogleButtonComponent } from "./google-button/google.button.component";
import { GameActionTranslationPipe } from "./pipes/gameActionTranslationPipe";
import { DateFilterPipe } from "./pipes/dateFilterPipe";
import { UserRoleComponent } from "./user-role/user-role.component";
import { UploadImageComponent } from './upload-image/upload-image.component';
import { DualListComponent } from './dual-list/dual-list.component';
import { ListboxModule } from 'primeng/primeng';
import { FormsModule } from '@angular/forms';
import { GoogleSigninComponent } from './google-signin/google-signin.component';

@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ListboxModule],
  declarations: [GeocoordinateComponent, GameActionTranslationPipe,
    DateFilterPipe, UserRoleComponent, DualListComponent,
    UploadImageComponent, GoogleSigninComponent],
  bootstrap: [GeocoordinateComponent, GoogleSigninComponent, UserRoleComponent, UploadImageComponent,
    DualListComponent],
  exports: [GeocoordinateComponent, GoogleSigninComponent, GameActionTranslationPipe, DateFilterPipe,
    UserRoleComponent, DualListComponent],


})
export class SharedModule {
}
