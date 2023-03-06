using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApplication5.Data.Models;

namespace WebApplication5.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
		public DbSet<Book> Books { get; set; }
		public DbSet<BuyKeys> BuyKeys { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    }
}