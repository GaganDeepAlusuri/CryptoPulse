using CryptoPulse.Areas.Identity.Data;
using CryptoPulse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Text;
using Microsoft.EntityFrameworkCore;

namespace CryptoPulse.Data
{
    public class IdentityDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Coin> Coins { get; set; }
        public DbSet<Market> Markets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coin>()
                .HasOne(coin => coin.IdentityUser)
                .WithMany(user => user.Coins)
                .HasForeignKey(coin => coin.IdentityUserId);

            modelBuilder.Entity<Coin>()
                .HasMany(c => c.Markets)
                .WithOne(m => m.Coin)
                .HasForeignKey(m => m.coinId);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
        }
    }
}