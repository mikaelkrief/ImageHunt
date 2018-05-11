using ImageHuntWebServiceClient.WebServices;

namespace ImageHuntTelegramBot.Dialogs
{
  public class ReceiveImageDialog : AbstractDialog, IReceiveImageDialog
    {
      private readonly ITeamWebService _teamWebService;

      public ReceiveImageDialog(ITeamWebService teamWebService)
      {
        _teamWebService = teamWebService;
      }
    }
}
