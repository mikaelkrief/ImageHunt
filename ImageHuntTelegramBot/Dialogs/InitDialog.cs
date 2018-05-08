using ImageHuntTelegramBot.Dialogs.Prompts;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImageHuntTelegramBot.Dialogs
{
  public class InitDialog : AbstractDialog, IInitDialog
  {
    public InitDialog()
    {
      var firstStep = new NumberPrompt<int>("Merci de m'indiquer l'id de la partie");
      AddChildren(firstStep);
      var secondStep = new NumberPrompt<int>("Merci de m'indiquer l'id de l'équipe");
      AddChildren(secondStep);
    }
  }
}