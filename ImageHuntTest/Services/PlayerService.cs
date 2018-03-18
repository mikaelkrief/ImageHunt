﻿using System;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Exception;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using ImageHuntCore.Services;
using Action = ImageHunt.Model.Action;
using Game = Telegram.Bot.Types.Game;

namespace ImageHuntTest
{
  public class PlayerService : AbstractService, IPlayerService
  {

    public PlayerService(HuntContext context) : base(context)
    {
    }

    public Player CreatePlayer(string name, string chatLogin)
    {
      var player = new Player(){Name = name, ChatLogin = chatLogin};
      Context.Players.Add(player);
      Context.SaveChanges();
      return player;
    }

    public Player JoinTeam(string gameName, string teamName, string playerName)
    {
      var game = Context.Games.SingleOrDefault(g => g.Name == gameName);
      if (game == null)
        throw new ArgumentException($"Game {gameName} doesn't exist");
      var team = game.Teams.SingleOrDefault(t => t.Name == teamName);
      if (team == null)
        throw new ArgumentException($"Team {teamName} doesn't exist");
      var player = GetPlayer(playerName);
      if (team !=null && player != null)
      {
        team.Players.Add(player);
        Context.SaveChanges();
      }

      return player;
    }

    private Player GetPlayer(string playerName)
    {
      var player = Context.Players.SingleOrDefault(p => p.Name == playerName);
      if (player == null)
        throw new ArgumentException($"Player {playerName} doesn't exist");

      return player;
    }
    public void StartPlayer(string name)
    {
      var player = GetPlayer(name);
      var game = player.CurrentGame;
      if (game.StartDate.Value.Date != DateTime.Today || !game.IsActive)
        throw new ArgumentException("There is no game active or today");
      player.StartTime = DateTime.Now;
      player.CurrentNode = game.Nodes.FirstOrDefault(n => n is FirstNode);
      Context.SaveChanges();
    }

    public Node NextNodeForPlayer(string playerName, double playerLatitude, double playerLongitude)
    {
      var player = GetPlayer(playerName);
      if (player.CurrentGame == null || !player.CurrentGame.IsActive)
        throw new InvalidGameException();
      var nextNode = player.CurrentNode.Children.First();
      var gameAction = new GameAction()
      {
        DateOccured = DateTime.Now,
        Player = player,
        Game = player.CurrentGame,
        Longitude = playerLongitude,
        Latitude = playerLatitude,
        Node = player.CurrentNode
      };
      player.CurrentNode = nextNode;
      Context.GameActions.Add(gameAction);
      Context.SaveChanges();
      return nextNode;
    }

    public void UploadImage(string playerName, double latitude, double longitude, byte[] image)
    {
      if (image == null)
        throw new ArgumentException("Parameter image is not provided");
      var player = GetPlayer(playerName);
      var gameAction = new GameAction()
      {
        DateOccured = DateTime.Now,
        Game = player.CurrentGame,
        Player = player,
        Latitude = latitude,
        Longitude = longitude,
        Picture = new Picture() { Image = image},
        Action = Action.SubmitPicture
      };
      Context.GameActions.Add(gameAction);
      Context.SaveChanges();
    }
  }
}