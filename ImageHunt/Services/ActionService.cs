using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHuntCore.Computation;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntCore.Services;
using ImageHuntWebServiceClient.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Action = ImageHuntCore.Model.Action;

namespace ImageHunt.Services
{
  public class ActionService : AbstractService, IActionService
  {
    private readonly IScoreChanger _scoreChanger;

    public async Task<PaginatedList<GameAction>> GetGameActionsForGame(int gameId, int pageIndex, int pageSize, IncludeAction includeAction, int? teamId = null)
    {
      var gameActions = Context.GameActions
          .Include(ga => ga.Game)
          .Include(ga => ga.Team)
          .Include(ga => ga.Node)
          .Include(ga => ga.Picture)
          .Where(ga => ga.Game.Id == gameId)
          .Where(ga => !ga.IsReviewed)
        ;
      if (teamId.HasValue)
        gameActions = gameActions.Where(ga => ga.Team.Id == teamId.Value);
      switch (includeAction)
      {
        case IncludeAction.Picture:
          gameActions = gameActions
            .Where(ga => ga.Action == Action.SubmitPicture)
            .Where(ga => ga.Latitude.HasValue && ga.Longitude.HasValue)


            ;
          break;
        case IncludeAction.ReplyQuestion:
          break;
        case IncludeAction.HiddenNode:
          gameActions = gameActions
            .Where(ga => ga.Action == Action.HiddenNode || ga.Action == Action.BonusNode);
          break;
      }
      foreach (var gameAction in gameActions)
      {
        gameAction.Delta = ComputeDelta(gameAction);
        if (gameAction.Node != null && gameAction.Node.NodeType == "PictureNode")
        {
          gameAction.Node = Context.PictureNodes.Include(p => p.Image)
            .Single(p => p.Id == gameAction.Node.Id);
          // Only send id of the picture
          var pictureNode = gameAction.Node as PictureNode;
          if (pictureNode != null && pictureNode.Image != null)
            pictureNode.Image.Image = null;
        }
      }

      return await PaginatedList<GameAction>.CreateAsync(gameActions, pageIndex, pageSize);
    }
    public int GetGameActionCountForGame(int gameId, IncludeAction includeAction, int? teamId = null)
    {
      int gameActionCountForGame = 0;
      var gameActions = Context.GameActions
          .Include(ga => ga.Game)
          .Include(ga => ga.Team)
          .Include(ga => ga.Node)
          .Include(ga => ga.Picture)
          .Where(ga => ga.Game.Id == gameId)
          .Where(ga => !ga.IsReviewed)
        ;

      switch (includeAction)
      {
        case IncludeAction.Picture:
          gameActions = gameActions
              .Where(ga => ga.Action == Action.SubmitPicture)
              .Where(ga => ga.Latitude.HasValue && ga.Longitude.HasValue)
            ;
          break;
      }

      if (teamId.HasValue)
        gameActions = gameActions.Where(ga => ga.Team.Id == teamId.Value);
      var count = gameActions.Count();
      return gameActions.Count();
    }

    protected virtual double ComputeDelta(GameAction gameAction)
    {
      if (gameAction.Node != null)
      {
        var delta = GeographyComputation.Distance(gameAction.Node.Latitude, gameAction.Node.Longitude,
          gameAction.Latitude.Value, gameAction.Longitude.Value);
        _logger.LogDebug("Delta = {0} for nodeId {1}", delta, gameAction.Node.Id);
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

    public GameAction Validate(int actionId, int nodeId, int validatorId, bool validate)
    {
      var gameAction = Context.GameActions
        .Include(ga => ga.Node)
        .Single(ga => ga.Id == actionId);
      var validator = Context.Admins.Single(a => a.Id == validatorId);
      var node = Context.Nodes.SingleOrDefault(n => n.Id == nodeId);
      gameAction.Reviewer = validator;
      gameAction.IsReviewed = true;
      gameAction.DateReviewed = DateTime.Now;
      gameAction.IsValidated = validate;
      gameAction.PointsEarned = gameAction.IsValidated ? node?.Points ?? 0 : 0;
      Context.SaveChanges();
      return gameAction;
    }

    public ActionService(HuntContext context, ILogger<ActionService> logger, IScoreChanger scoreChanger)
      : base(context, logger)
    {
      _scoreChanger = scoreChanger;
    }

    public void AddGameAction(GameAction gameAction)
    {
      var game = Context.Games.Single(g => g.Id == gameAction.Game.Id);
      var team = Context.Teams.Single(t => t.Id == gameAction.Team.Id);
      var node = gameAction.Node !=null ? Context.Nodes.Single(n => n.Id == gameAction.Node.Id) : null;
      Picture picture = null;
      if (gameAction.Picture != null && gameAction.Picture.Id != 0)
        picture = Context.Pictures.Single(p => p.Id == gameAction.Picture.Id);
      gameAction.Game = game;
      gameAction.Team = team;
      gameAction.Node = node;
      gameAction.Picture = picture;
      Context.GameActions.Add(gameAction);
      Context.SaveChanges();
    }


    public IEnumerable<Score> GetScoresForGame(int gameId)
    {
      var game = Context.Games.Single(g => g.Id == gameId);
      var scoresForGame = Context.GameActions.Include(ga => ga.Game)
        .Include(ga => ga.Team).ThenInclude(t => t.TeamPlayers).ThenInclude(tp => tp.Player)
        .Where(ga => ga.Game.Id == gameId && ga.IsValidated)
        .GroupBy(ga => ga.Team)
        /// TODO remove this ToList in dotnet 3.0
        .ToList()
        .Select(g => new Score() { Team = g.Key, Points = g.Sum(_ => _.PointsEarned) }).ToList();
      scoresForGame = scoresForGame.Where(s => s.Team != null).ToList();
      foreach (var score in scoresForGame)
      {
        var startDate = Context.GameActions.LastOrDefault(ga => ga.Team == score.Team && ga.Action == Action.StartGame)
          ?.DateOccured;
        var endDate = Context.GameActions.FirstOrDefault(ga => ga.Team == score.Team && ga.Action == Action.EndGame)
          ?.DateOccured;
        score.Points = _scoreChanger.ComputeScore(score, game);
        if (startDate.HasValue && endDate.HasValue)
        {
          score.TravelTime = endDate.Value - startDate.Value;
        }
      }
      return scoresForGame;
    }

    public IEnumerable<GameAction> GetGamePositionsForGame(int gameId)
    {
      return Context.GameActions
        .Include(ga => ga.Game)
        .Include(ga => ga.Team)
        .Where(ga => ga.Game.Id == gameId && ga.Latitude.HasValue && ga.Longitude.HasValue);
    }

    public GameAction Update(GameAction gameAction)
    {
      Context.Attach(gameAction);
      Context.SaveChanges();
      return gameAction;
    }
  }
}
