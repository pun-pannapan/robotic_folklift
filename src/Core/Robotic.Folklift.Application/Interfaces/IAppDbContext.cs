using Microsoft.EntityFrameworkCore;
using Robotic.Folklift.Domain.Entities;

namespace Robotic.Folklift.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Forklift> Forklifts { get; }
        DbSet<FolkliftCommand> FolkliftCommands { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
