import { NgModule } from '@angular/core';
import {GameActionTranslationPipe} from "./gameActionTranslationPipe";

@NgModule({
  declarations:[GameActionTranslationPipe],
  exports: [GameActionTranslationPipe]
})
export class PipeModule {
  static forRoot() {
    return {
      ngModule: PipeModule,
      providers: []

    };
  }
}
