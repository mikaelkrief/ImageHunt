import { Pipe, PipeTransform } from "@angular/core";
import * as moment from 'moment';

@Pipe({name:"momentFormat"})
export class MomentFormatPipe implements PipeTransform {
  transform(item: any, format: string) {
    let duration = moment.duration(item);
    let today = moment(0, 'HH');
    return today.add(duration).format('HH:mm:ss');
  }
}
