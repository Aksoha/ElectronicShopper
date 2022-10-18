using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess;

public interface IUserData
{
    Task<UserModel?> GetById(int id);
    Task CreateUser(UserModel user);
}