using System;
using System.Collections.Generic;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;

namespace ImageHunt.Services
{
    public class GameService : AbstractService
    {
        public GameService(HuntContext context): base(context)
        {
        }

        public Game CreateGame(string gameName, DateTime startDate, List<Node> nodes)
        {
            if (!nodes.Any(n=>n.GetType() == typeof(FirstNode)))
              throw new ArgumentException("There is no first node in the list of node");
            var game = new Game()
            {
                Name = gameName,
                StartDate = startDate,
                Nodes = nodes,
            };
            Context.Games.Add(game);
            Context.SaveChanges();
            return game;
        }
    }
}
