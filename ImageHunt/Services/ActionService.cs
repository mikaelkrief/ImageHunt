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
            .Where(ga => ga.Game.Id == gameId);
        foreach (var gameAction in gameActions)
        {
          gameAction.Delta = ComputeDelta(gameAction);
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

      public List<GameAction> GetValidatedGameActionForGame(int gameId)
      {
        return Context.GameActions.Where(ga => ga.Game.Id == gameId && ga.IsValidated).ToList();
      }
    }
}
