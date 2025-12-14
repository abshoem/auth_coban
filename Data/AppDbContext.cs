using CrudAuthenAuthortruyenthong.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudAuthenAuthortruyenthong.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {  get; set; }
        public DbSet<RefreshToken> RefreshTokens {  get; set; }

        



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(users => users.Id);
                e.HasIndex(name => name.Username).IsUnique();
                e.Property(uname => uname.Username).IsRequired();
                e.Property(pass => pass.PasswordHash).IsRequired();
                e.Property(role => role.Role).IsRequired().HasConversion<string>();
               
              
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");
                entity.HasKey(refreshTokens => refreshTokens.Id);
                entity.Property(r => r.Token)
                   .IsRequired();
                   

                entity.Property(r => r.Expires)
                    .IsRequired();
                entity.HasOne(r => r.User)
                        .WithMany(u => u.RefreshTokens)
                        .HasForeignKey(r => r.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
