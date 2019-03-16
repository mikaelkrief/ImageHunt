import * as moment from "moment";

@Pipe({ name: "momentFormat" })
export class MomentFormatPipe implements PipeTransform {
  transform(item: any, format: string) {
    const duration = moment.duration(item);
    const today = moment(0, "HH");
    return today.add(duration).format("HH:mm:ss");
  }
}
