using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Computation;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHuntCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Action = ImageHuntWebServiceClient.Action;

namespace ImageHunt.Services
{
    public class ActionService : AbstractService, IActionService
    {
      public async Task<PaginatedList<GameAction>> GetGameActionsForGame(int gameId, int pageIndex, int pageSize)
      {
        var gameActions = Context.GameActions
            .Include(ga => ga.Game)
            .Include(ga => ga.Team)
            .Include(ga => ga.Node)
            .Include(ga => ga.Picture)
            .Where(ga=>ga.Action !=  Action.SubmitPosition)
            .Where(ga => ga.Game.Id == gameId);

        foreach (var gameAction in gameActions)
        {
          gameAction.Delta = ComputeDelta(gameAction);
          if (gameAction.Node != null && gameAction.Node.NodeType == "PictureNode")
          {
            gameAction.Node = Context.PictureNodes.Include(p => p.Image)
              .Single(p => p.Id == gameAction.Node.Id);
            // Only send id of the picture
          ((PictureNode)gameAction.Node).Image.Image = null;
          }
        }
        return await PaginatedList<GameAction>.CreateAsync(gameActions, pageIndex, pageSize);
      }

      protected virtual double ComputeDelta(GameAction gameAction)
      {
        if (gameAction.Node != null)
        {
          var delta = GeographyComputation.Distance(gameAction.Node.Latitude, gameAction.Node.Longitude,
            gameAction.Latitude, gameAction.Longitude);
          _logger.LogDebug($"Delta = {delta} for nodeId {gameAction.Node.Id}");
          return delta;
        }
      else
        {
          return double.NaN;
        }
      }

      public GameAction GetGameAction(int gameActionId)
      {
        var gameAction = Context.GameActions
          .Include(ga => ga.Game)
          .Include(ga => ga.Team)
          .Include(ga => ga.Node)
          .Include(ga => ga.Picture)
          .Single(ga => ga.Id == gameActionId);
        gameAction.Delta = ComputeDelta(gameAction);
        return gameAction;
      }

      public void Validate(int actionId, int validatorId)
      {
        var gameAction = Context.GameActions
          .Include(ga=>ga.Node)
          .Single(ga => ga.Id == actionId);
        var validator = Context.Admins.Single(a => a.Id == validatorId);
        gameAction.Reviewer = validator;
        gameAction.IsReviewed = true;
        gameAction.DateReviewed = DateTime.Now;
        gameAction.IsValidated = !gameAction.IsValidated;
        gameAction.PointsEarned = gameAction.IsValidated?gameAction.Node?.Points ?? 0:0;
        Context.SaveChanges();
      }

      public ActionService(HuntContext context, ILogger<ActionService> logger)
        : base(context, logger)
      {
      }

      public void AddGameAction(GameAction gameAction)
      {
        Context.GameActions.Add(gameAction);
        Context.SaveChanges();
      }

      public int GetGameActionCountForGame(int gameId)
      {
        return Context.GameActions.Count(ga => ga.Game.Id == gameId);
      }

      public IEnumerable<Score> GetScoresForGame(int gameId)
      {
        return Context.GameActions.Include(ga => ga.Game)
          .Include(ga => ga.Team).ThenInclude(t=>t.TeamPlayers).ThenInclude(tp=>tp.Player)
          .Where(ga => ga.Game.Id == gameId && ga.IsValidated)
          .GroupBy(ga => ga.Team)
          .Select(g=>new Score(){Team = g.Key, Points = g.Sum(_=>_.PointsEarned)});
      }
    }
}
