using System.Collections.Generic;
using ImageHunt.Model;

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
  }
}
