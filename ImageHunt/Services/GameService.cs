using System;
using System.Collections.Generic;
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

        public Game CreateGame(string gameName, DateTime startDate, List<Node> nodes, Node firstNode)
        {
            if (!nodes.Contains(firstNode))
                throw new ArgumentException("The first node is not contained in nodes", nameof(firstNode));
            var game = new Game()
            {
                Name = gameName,
                StartDate = startDate,
                Nodes = nodes,
                FirstNode = firstNode
            };
            Context.Games.Add(game);
            Context.SaveChanges();
            return game;
        }
    }
}