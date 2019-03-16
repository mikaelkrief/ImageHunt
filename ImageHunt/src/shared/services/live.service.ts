import * as signalR from "@aspnet/signalr";

@Injectable()
export class LiveService {
  _connection: any;

  constructor() {
    this._connection = new signalR.HubConnectionBuilder()
      .withUrl("/locationHub").build();
  }

}
