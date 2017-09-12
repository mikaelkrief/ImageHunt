using System;
using System.Collections.Generic;
using ImageHunt.Model;
using ImageHunt.Model.Node;

namespace ImageHunt.Services
{
  public interface IGameService : IService
  {
    Game CreateGame(string gameName, DateTime startDate, List<Node> nodes);
    Game GetGameById(int gameId);
  }
}