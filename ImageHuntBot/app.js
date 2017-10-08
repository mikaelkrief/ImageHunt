"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const restify = require("restify");
const builder = require("botbuilder");
const dialog_1 = require("./dialog");
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, () => console.log(`${server.name} listening to ${server.url}`));
var connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});
// Listen for messages from users 
server.post('/bot/messages', connector.listen());
var bot = new builder.UniversalBot(connector, dialog_1.default);
