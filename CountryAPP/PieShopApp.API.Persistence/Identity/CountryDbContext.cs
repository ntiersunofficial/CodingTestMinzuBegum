using CountryAPP.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace CountryAPP.API.Persistence.Identity;

public class CountryDbContext: DbContext
{
    public CountryDbContext(DbContextOptions<CountryDbContext> options) : base(options) { }

    public DbSet<UserModel> Users { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    // Configure your entities here
    //    modelBuilder.Entity<UserModel>(entity =>
    //    {
    //        entity.HasKey(e => e.Email);
           
    //        // Add other configurations
    //    });

    //    // Add similar configuration for CountryModel if needed
    //}

}
