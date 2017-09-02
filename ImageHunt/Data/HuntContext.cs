using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Model;
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
        public DbSet<TimerNode> TimerNodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
