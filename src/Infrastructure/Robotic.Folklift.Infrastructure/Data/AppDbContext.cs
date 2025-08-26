using Microsoft.EntityFrameworkCore;
using Robotic.Folklift.Application.Interfaces;
using Robotic.Folklift.Domain.Entities;

namespace Robotic.Folklift.Infrastructure.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
        public DbSet<Forklift> Forklifts => Set<Forklift>();
        public DbSet<FolkliftCommand> FolkliftCommands => Set<FolkliftCommand>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<Forklift>().Property(x => x.Name).HasMaxLength(200).IsRequired();
            modelBuilder.Entity<Forklift>().Property(x => x.ModelNumber).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<FolkliftCommand>().HasOne(x => x.Forklift).WithMany(f => f.Commands).HasForeignKey(x => x.ForkliftId);
            modelBuilder.Entity<FolkliftCommand>().HasOne(x => x.IssuedBy).WithMany().HasForeignKey(x => x.IssuedByUserId);
        }
    }
}
