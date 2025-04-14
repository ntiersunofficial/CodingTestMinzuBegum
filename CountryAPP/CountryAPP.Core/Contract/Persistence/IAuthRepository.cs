using CountryAPP.Core.Model;

namespace CountryAPP.Core.Contract.Persistence;

public interface IAuthRepository
{
	Task<UserInfoModel> GetCurrentUser(string userId);

    Task<bool> IsAuthenticatedUser(UserLoginModel lg);

    Task<TokenModel> GetRefreshToken(string userId);

	Task UpdateRefreshToken(string userId, TokenModel token);
}