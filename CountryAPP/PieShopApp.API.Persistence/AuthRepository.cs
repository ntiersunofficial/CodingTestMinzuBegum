using CountryAPP.API.Persistence.Identity;
using CountryAPP.Core.Contract.Infrastructure;
using CountryAPP.Core.Contract.Persistence;
using CountryAPP.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryAPP.API.Persistence;

public class AuthRepository : IAuthRepository
{
    private readonly CountryDbContext _context;
    private readonly ISecurityHelper _securityHelper;

    public AuthRepository(CountryDbContext context, ISecurityHelper securityHelper)
    {
        _context = context;
        _securityHelper = securityHelper;
    }

    public async Task<UserInfoModel> GetCurrentUser(string userId)
    {
        
        return await _context.Users
            .Where(u => u.Email == userId)
            .Select(u => new UserInfoModel
            {
                Id = u.Id,
                Name = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsAuthenticatedUser(UserLoginModel lg)
    {
      
        var user = await _context.Users
            .Where(u => u.Email == lg.UserName)
            .Select(u => new
            {
                u.Password             })
            .FirstOrDefaultAsync();

        if (user?.Password == null) return false;

        // Verify password against stored hash
        return _securityHelper.IsValidHash(user.Password, lg.Password);
    }

    public async Task<TokenModel> GetRefreshToken(string userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new TokenModel
            {
                RefreshToken = u.RefreshToken,
                RefreshTokenExpires = u.RefreshTokenExpires // if you need this too
            })
            .FirstOrDefaultAsync();
    }

    public async Task UpdateRefreshToken(string userId, TokenModel token)
    {
        //DynamicParameters p = new DynamicParameters();
        //p.Add("UserId", userId);
        //p.Add("RefreshToken", token.RefreshToken);
        //p.Add("RefreshTokenExpires", token.RefreshTokenExpires);

        //await _dataAccessHelper.ExecuteData("USP_AspNetUsers_TokenUpdate", p);
    }
}
