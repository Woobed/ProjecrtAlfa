using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectAlfa.Entities.AuthorizationEntities;

namespace ProjectAlfa.ApplicationDbContext;

public class AppDbContext: IdentityDbContext<IdentityUser>
	
{
	public new DbSet<User> Users { get; set; }
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        
        
        base.OnModelCreating(builder);
    }

}