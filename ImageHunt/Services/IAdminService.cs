using System.Collections.Generic;
using ImageHuntCore.Model;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
  public interface IAdminService : IService
  {
    /// <summary>
    /// Get all admins with associated games
    /// </summary>
    IEnumerable<Admin> GetAllAdmins();
    /// <summary>
    /// Insert admin in database
    /// </summary>
    void InsertAdmin(Admin admin);
    /// <summary>
    /// Delete admin from database
    /// </summary>
    void DeleteAdmin(Admin adminToDelete);
    /// <summary>
    /// Get admin from database using id
    /// </summary>
    Admin GetAdminById(int adminId);
    /// <summary>
    /// Get admin from database using his email
    /// </summary>
    Admin GetAdminByEmail(string email);

    /// <summary>
    /// Assign a game to an admin
    /// </summary>
    /// <param name="adminId">Admin Id</param>
    /// <param name="gameId">GameId</param>
    /// <param name="assign"></param>
    /// <returns></returns>
    Admin AssignGame(int adminId, int gameId, bool assign);

    Admin GetAdminByUserName(string userName);
  }
}
