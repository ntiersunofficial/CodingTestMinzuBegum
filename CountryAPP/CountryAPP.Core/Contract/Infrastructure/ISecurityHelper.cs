using CountryAPP.Core.Model;

namespace CountryAPP.Core.Contract.Infrastructure;

public interface ISecurityHelper
{
	string GenerateHash(string payload = "Default Payload");
	string GenerateJSONWebToken(UserInfoModel userInfo);
	string GenerateRefreshToken();
	bool IsValidHash(string senderHash, string payLoad = "Default Payload");
}