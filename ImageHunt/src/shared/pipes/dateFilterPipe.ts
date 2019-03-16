import { Pipe, PipeTransform } from "@angular/core";

@Pipe({name:"dateFilter"})
export class DateFilterPipe implements PipeTransform {
  transform(items: any[], field: string, today: Date, before: boolean): any {
    let todayWithoutTime = new Date(today.setHours(0, 0, 0, 0));
    if (items) {
      if (before)
        return items.filter(item => new Date(item[field]) < todayWithoutTime);
      else
        return items.filter(item => new Date(item[field]) >= todayWithoutTime);
    }
  }
}
