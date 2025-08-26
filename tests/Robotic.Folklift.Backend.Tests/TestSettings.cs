using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Robotic.Folklift.Application.Mappings;
using Robotic.Folklift.Infrastructure.Data;

namespace Robotic.Folklift.Backend.Tests;

public static class TestSettings
{
    public static AppDbContext NewDb(string? name = null)
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(name ?? Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;
        return new AppDbContext(opts);
    }

    public static IMapper NewMapper()
    {
        var cfg = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
        return cfg.CreateMapper();
    }
}
