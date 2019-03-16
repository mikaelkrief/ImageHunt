@Injectable()
export class AlertService {
  constructor() {}

  alerts: any = [];

  sendAlert(msg: string, type: string, timeout: number) {
    this.alerts.push({ type: type, message: msg, timeout: timeout, time: new Date() });
  }
}
