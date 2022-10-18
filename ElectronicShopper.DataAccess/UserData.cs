using AutoMapper;
using ElectronicShopper.DataAccess.Models.Internal;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _sql;
    private readonly IMapper _mapper;
    private const string ConnectionStringName = "ElectronicShopperData";

    public UserData(ISqlDataAccess sql, IMapper mapper)
    {
        _sql = sql;
        _mapper = mapper;
    }

    public async Task<UserModel?> GetById(int id)
    {
        _sql.StartTransaction(ConnectionStringName);
        var result = await _sql.LoadData<UserDbModel, dynamic>("dbo.spUser_GetById", new { Id = id });
        _sql.CommitTransaction();

        var output = _mapper.Map<UserModel>(result.FirstOrDefault());
        return output;
    }

    public async Task CreateUser(UserModel user)
    {
        _sql.StartTransaction(ConnectionStringName);
        var id = await _sql.SaveData<dynamic, int>("dbo.spUser_Insert", new { user.UserId, user.FirstName, user.LastName, user.Email });
        _sql.CommitTransaction();

        user.Id = id;
    }
}