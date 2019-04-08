# Front End
* Install last version of node.js
`https://nodejs.org/en/`
* Install build tools for npm
`npm install --global --production windows-build-tools`

* Install angular environment:
`npm install -g @angular/cli@1.7.4`

* Install typings:
`npm install typings --global`

* Install google map lib
`typings install dt~google.maps --global --save`

* Install dotnet core:
`https://www.microsoft.com/net/download/windows`

* Clone the repository:
`git clone https://github.com/nimbusparis/ImageHunt`

* Install database server

* Create secret config files
1. Appsettings.Development.json
From `./Appsettings.template.json` create your own with the login/password & API key.

2. environment.ts
From `./src/environments/environment.template.ts` create your own `environment.ts`. `environment.prod.ts` should be created on the same pattern for production environment.

* Compile
1. `dotnet build` 
For webapi

2. `ng build`
For front-end (try `ng build --watch` if you want continuous build while you coding.

* Launch
If you have Visual Studio, it should launch the application with IIS Express or embedded web host. 
If you want to launch it by command line, use the following command:

`dotnet ImageHunt.dll --urls "http://localhost:<ListenPort>" --environment <YourEnvName>`


# Telegram Bot
The bot is a WebAPI that communicate with Telegram servers using a WebHook. First, you need a bot token, ask to `@botfather`.
Then, configure your webhook, doing a post on this url:

`https://api.telegram.org/bot<BotToken>/setWebhook` with form-data: `url:<url_of_your_bot>`

Use `ngrok` if you need to redirect port on you dev computer:

    ngrok http <bot_port>

This will redirect all call, any port on `<bot_port>`. `ngrok` will log all call and can replay it. You can also extract the request and replay it using tools as `Postman`.
