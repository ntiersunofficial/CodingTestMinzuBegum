using CountryAPP.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryAPP.Core.Contract.Persistence;

public interface IAccountRepository
{
    Task<List<UserInfoModel>> GetAllUserInfoAsync();
    Task<UserInfoModel> GetByIdAsync(string id);
    Task<UserModel> CreateOrEditUser(UserModel user);
    Task<int> DeleteUser(int id);
    //Task<Response> ChangePasswordSelf(ChangePasswordModel changePasswordRequestSelf);
}
