using System.Diagnostics.CodeAnalysis;
using System.IO;
using ImageHuntCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ImageHunt.Data
{
  public class HuntContextDesignFactory : IDesignTimeDbContextFactory<HuntContext>
    {
        public HuntContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
      var optionsBuilder = new DbContextOptionsBuilder<HuntContext>();
      optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
      return ActivableContext<HuntContext>
              .CreateInstance(optionsBuilder.Options);
        }
    }
}
