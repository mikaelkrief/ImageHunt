using System;
using System.Linq;
using System.Text.RegularExpressions;
using ImageHunt.Data;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient.Responses;

namespace ImageHunt.Updater
{
  public class UpdateNodePointsUpdater : AbstractUpdater
  {

    public UpdateNodePointsUpdater(HuntContext context, Game game, string arguments) 
      : base(context, game, arguments)
    {
    }

    public override void Execute()
    {
      var nodeType = Arguments["nodeType"];
      Context.Attach(Game);
      switch (nodeType)
      {
        case NodeResponse.PictureNodeType:
          var regex = new Regex(Arguments["seedPattern"]);
          var nodes = Game.Nodes.Where(n => n.NodeType == nodeType);
          var multiplier = Convert.ToInt32(Arguments["multiplier"]);
          foreach (var node in nodes)
          {
            if (regex.IsMatch(node.Name))
            {
              var seed = Convert.ToInt32(regex.Matches(node.Name)[0].Groups["seed"].Value);
              node.Points = seed * multiplier;
            }
          }

          Context.SaveChanges();
          break;
      }
    }
  }
}
