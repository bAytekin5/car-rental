using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AracKiralama.Models.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
			ChangeTracker.LazyLoadingEnabled = false;
			ChangeTracker.AutoDetectChangesEnabled = false;
		}
		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<Car> Car { get; set; }
		public DbSet<Reservation> Reservation { get; set; }

	}
}
