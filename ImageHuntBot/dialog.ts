// dialog.ts
import * as builder from 'botbuilder';

interface IResults {
  response: string;
}
export default [
  (session: builder.Session) => {
    builder.Prompts.text(session, 'Bonjour, je suis un bot conversationnel qui vous permettra de progresser dans la chasse au trésor.' +
      'Je vous poserai des questions et vous indiquerai quel point rejoindre.' +
      'Pouvez-vous m\'envoyer votre position je vous indiquerai quelle est la chasse la plus appropriée.');
    //builder.Prompts.
  },
  (session: builder.Session, results: IResults) => {
    session.endConversation(`Hello, ${results.response}`);
  }
]