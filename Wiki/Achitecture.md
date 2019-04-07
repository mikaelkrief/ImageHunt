The application is divided in 3 parts:
* Front-end using Angular 2.0x+
* Chatbot using dotnet core 2.x+
* Back-end using dotnet core 2.x+

## Front-End
Developed with Angular 2.x+, the front end use various packages suche as primeng, ngx-bootstrap and leaflet. It communicate with REST API for persistance. A SignalR feed is also opened with backend to follow teams in real time.

## Back-end
It persist the data in a database through Entity Framework Core. It expose a signalR endpoint to follow players. The backed expose REST Api for clients (Front-end & Bot)

## Chatbot
It relies on [Bot Builder](https://github.com/Microsoft/BotBuilder) framework and [Telegram Bot](https://github.com/TelegramBots/Telegram.Bot) library. It replies on commands send by admin or players and interact with backend thru REST Api.

