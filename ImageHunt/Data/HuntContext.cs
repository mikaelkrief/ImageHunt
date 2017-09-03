using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ImageHunt.Data
{
    public class HuntContext : DbContext
    {
        public HuntContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<FirstNode> FirstNodes { get; set; }
        public DbSet<TimerNode> TimerNodes { get; set; }
        public DbSet<PictureNode> PictureNodes { get; set; }
        public DbSet<QuestionNode> QuestionNodes { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Answer> Answers { get; set; }

    }
}
