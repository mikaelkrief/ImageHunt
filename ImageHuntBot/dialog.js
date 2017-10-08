"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// dialog.ts
const builder = require("botbuilder");
exports.default = [
    (session) => {
        builder.Prompts.text(session, 'Bonjour, je suis un bot conversationnel qui vous permettra de progresser dans la chasse au trésor.' +
            'Je vous poserai des questions et vous indiquerai quel point rejoindre.' +
            'Pouvez-vous m\'envoyer votre position je vous indiquerai quelle est la chasse la plus appropriée.');
        //builder.Prompts.
    },
    (session, results) => {
        session.endConversation(`Hello, ${results.response}`);
    }
];
