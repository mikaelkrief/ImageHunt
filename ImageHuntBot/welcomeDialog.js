var builder = require('botbuilder');

module.exports = [
  function(session) {
    session.send('Bonjour ' + session + ', je suis le bot qui va vous aider à jouer à la chasse au trésor.');
  },
function(session) {
  session.send('vous faites partie de l\'équipe ');
  builder.Prompts.choice(session, 'Vous confirmez ?', ['Oui', 'Non']);
}
  ]