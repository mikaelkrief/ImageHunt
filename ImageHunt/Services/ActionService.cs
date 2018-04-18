using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Computation;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHuntCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Game = Telegram.Bot.Types.Game;

namespace ImageHunt.Services
{
    public class ActionService : AbstractService, IActionService
    {
      public IEnumerable<GameAction> GetGameActionsForGame(int gameId)
      {
        var gameActions = Context.GameActions
            .Include(ga => ga.Game).Include(ga => ga.Player).Include(ga => ga.Node)
            .Where(ga => ga.Game.Id == gameId)
          ;
        foreach (var gameAction in gameActions)
        {
          gameAction.Delta = ComputeDelta(gameAction);
        }
        return gameActions;
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
          .Include(ga => ga.Game).Include(ga => ga.Player).Include(ga => ga.Node)
          .Single(ga => ga.Id == gameActionId);
        gameAction.Delta = ComputeDelta(gameAction);
        return gameAction;
      }

      public void Validate(int actionId, int validatorId)
      {
        var gameAction = Context.GameActions.Include(ga=>ga.Node).Single(ga => ga.Id == actionId);
        var validator = Context.Admins.Single(a => a.Id == validatorId);
        gameAction.Reviewer = validator;
        gameAction.IsReviewed = true;
        gameAction.DateReviewed = DateTime.Now;
        gameAction.IsValidated = !gameAction.IsValidated;
        gameAction.PointsEarned = gameAction.Node?.Points ?? 0;
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
    }
}
