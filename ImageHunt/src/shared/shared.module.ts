import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { GeocoordinateComponent } from "./geocoordinate/geocoordinate.component";
import { RouterModule } from "@angular/router";
import { GameActionTranslationPipe } from "./pipes/gameActionTranslationPipe";
import { DateFilterPipe } from "./pipes/dateFilterPipe";
import { UserRoleComponent } from "./user-role/user-role.component";
import { UploadImageComponent } from './upload-image/upload-image.component';
import { DualListComponent } from './dual-list/dual-list.component';
import { ListboxModule } from 'primeng/primeng';
import { FormsModule } from '@angular/forms';
import { LoginButtonComponent } from './login-button/login-button.component';
import { SortPipe } from "./pipes/sortPipe";
import { MomentFormatPipe } from "./pipes/momentFormatPipe";
import { UppyComponent } from './uppy/uppy.component';
@NgModule({
  imports: [CommonModule, RouterModule, FormsModule, ListboxModule],
  declarations: [GeocoordinateComponent, GameActionTranslationPipe,
    DateFilterPipe, UserRoleComponent, DualListComponent, SortPipe, MomentFormatPipe,
    UploadImageComponent, LoginButtonComponent, UppyComponent],
  bootstrap: [GeocoordinateComponent, UserRoleComponent, UploadImageComponent,
    DualListComponent, LoginButtonComponent, UppyComponent],
  exports: [GeocoordinateComponent, GameActionTranslationPipe, DateFilterPipe, SortPipe, MomentFormatPipe,
    UserRoleComponent, DualListComponent, LoginButtonComponent, UppyComponent],


})
export class SharedModule {
}
