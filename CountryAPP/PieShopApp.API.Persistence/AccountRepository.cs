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

public class AccountRepository : IAccountRepository
{
    private readonly CountryDbContext _context;
    private readonly ISecurityHelper _securityHelper;

    public AccountRepository(CountryDbContext context, ISecurityHelper securityHelper)
    {
        _context = context;
        _securityHelper = securityHelper;
    }

    public async Task<List<UserInfoModel>> GetAllUserInfoAsync()
    {
        var users = await _context.Users
            .Where(u => u.IsActive == 0) 
            .Select(u => new UserInfoModel
            {
                Id = u.Id,
                Name = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive
            })
            .ToListAsync();

        return users;
    }


    public async Task<UserInfoModel> GetByIdAsync(string id)
    {
        return await _context.Users
         .Where(u => u.Id == id)
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

    public async Task<UserModel> CreateOrEditUser(UserModel model)
    {
        UserModel entity;

        if (!string.IsNullOrEmpty(model.Id)) // Update existing user
        {
            entity = await _context.Users.FindAsync(model.Id);

            if (entity == null) throw new Exception("User not found");

            entity.FullName = model.FullName;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Email = model.Email; // optionally update Email
            //entity.Password = _securityHelper.GenerateHash(model.Password); 
        }
        else // Create new user
        {
            // Check if user already exists by email
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
            {
                throw new Exception("User already exists with this email.");
            }

            entity = new UserModel
            {
                Id = Guid.NewGuid().ToString(),
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = _securityHelper.GenerateHash(model.Password),
                IsActive = 0
            };

            await _context.Users.AddAsync(entity);
        }

        await _context.SaveChangesAsync();

        model.Id = entity.Id;
        return model;
    }

    public async Task<int> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return 0;

        user.IsActive = 1; 
        _context.Users.Update(user);

        return await _context.SaveChangesAsync();
    }
}
