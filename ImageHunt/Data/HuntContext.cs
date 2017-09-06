using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
        base.OnModelCreating(modelBuilder);
        // Add property IsDeleted to all entities
        modelBuilder.Entity<Game>()
          .Property<bool>("IsDeleted");
        modelBuilder.Entity<Player>()
          .Property<bool>("IsDeleted");
        modelBuilder.Entity<Team>()
          .Property<bool>("IsDeleted");
        modelBuilder.Entity<FirstNode>()
          .Property<bool>("IsDeleted");
        modelBuilder.Entity<Node>()
          .Property<bool>("IsDeleted");
        modelBuilder.Entity<TimerNode>()
          .Property<bool>("IsDeleted");
        modelBuilder.Entity<PictureNode>()
          .Property<bool>("IsDeleted");
        modelBuilder.Entity<QuestionNode>()
          .Property<bool>("IsDeleted");
        modelBuilder.Entity<Answer>()
          .Property<bool>("IsDeleted");
        // Filter entities
        modelBuilder.Entity<Game>()
          .HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
        modelBuilder.Entity<Player>()
          .HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
      modelBuilder.Entity<Team>()
        .HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
      modelBuilder.Entity<Node>()
        .HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
      modelBuilder.Entity<Answer>()
        .HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
      {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
      
      }

      public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
      {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
      }

      private void OnBeforeSaving()
      {
        foreach (var entityEntry in ChangeTracker.Entries())
        {
          switch (entityEntry.State)
          {
            case EntityState.Deleted:
            entityEntry.State = EntityState.Modified;
              entityEntry.CurrentValues["IsDeleted"] = true;
              break;
            case EntityState.Added:
              entityEntry.CurrentValues["IsDeleted"] = false;
              break;
          }
        }
      }
    }
}
