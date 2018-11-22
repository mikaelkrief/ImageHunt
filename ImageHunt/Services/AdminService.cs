using System;
using System.Collections.Generic;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHuntCore.Model;
using ImageHuntCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImageHunt.Services
{
  public class AdminService : AbstractService, IAdminService
  {
    public AdminService(HuntContext context, ILogger<AdminService> logger) : base(context, logger)
    {
    }

    public IEnumerable<Admin> GetAllAdmins()
    {
      return Context.Admins.Include(a => a.GameAdmins).ThenInclude(ga=>ga.Game);
    }

    public void InsertAdmin(Admin admin)
    {
      Context.Admins.Add(admin);
      Context.SaveChanges();
    }

    public void DeleteAdmin(Admin adminToDelete)
    {
      Context.Admins.Remove(adminToDelete);
      Context.SaveChanges();
    }

    public Admin GetAdminById(int adminId)
    {
      return Context.Admins
        .Include(a => a.GameAdmins).ThenInclude(ga=>ga.Game)
        .Single(a => a.Id == adminId);
    }

    public Admin GetAdminByEmail(string email)
    {
      return Context.Admins
        .Include(g => g.GameAdmins).ThenInclude(ga => ga.Game)
        .Single(a => string.Equals(a.Email, email, StringComparison.InvariantCultureIgnoreCase));
    }

    public Admin AssignGame(int adminId, int gameId, bool assign)
    {
      var admin = Context.Admins.Include(a=>a.GameAdmins)
        .Single(a => a.Id == adminId);
      var game = Context.Games.Single(g => g.Id == gameId);
      if (assign)
      {
        if (admin.GameAdmins.Any(ga => ga.GameId == gameId && ga.AdminId == adminId))
          return admin;
        admin.GameAdmins.Add(new GameAdmin(){Admin = admin, Game = game});
        
      }
      else
      {
        var gameAdmin = admin.GameAdmins.SingleOrDefault(ga => ga.GameId == gameId && ga.AdminId == adminId);
        if (gameAdmin != null)
          admin.GameAdmins.Remove(gameAdmin);
      }
      Context.SaveChanges();
      return admin;
    }
  }
}
