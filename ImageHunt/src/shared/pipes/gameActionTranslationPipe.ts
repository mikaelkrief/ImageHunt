import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'gameActionTranslationPipe' })
export class GameActionTranslationPipe implements PipeTransform {
  transform(value: number) {
    switch (value) {
    case 0:
      return "Départ";
    case 1:
      return "Arrivée";
    case 2:
      return "Envoi de photo";
    case 3:
      return "Visite de lieu";
    }
  }

}
