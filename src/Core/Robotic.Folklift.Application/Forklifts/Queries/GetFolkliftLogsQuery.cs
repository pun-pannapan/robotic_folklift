using MediatR;
using Microsoft.EntityFrameworkCore;
using Robotic.Folklift.Application.Dtos;
using Robotic.Folklift.Application.Interfaces;
using System.Text.Json;

namespace Robotic.Folklift.Application.Forklifts.Commands
{
    public record GetFolkliftLogsQuery(int ForkliftId, PageQuery Query) : IRequest<PagedResult<FolkliftCommandDto>>;

    public class GetFolkliftLogsHandler : IRequestHandler<GetFolkliftLogsQuery, PagedResult<FolkliftCommandDto>>
    {
        private readonly IAppDbContext _db;
        public GetFolkliftLogsHandler(IAppDbContext db) { _db = db; }

        public async Task<PagedResult<FolkliftCommandDto>> Handle(GetFolkliftLogsQuery request, CancellationToken ct)
        {
            var (page, size, sortBy, dir) = request.Query;
            var folkliftCommand = _db.FolkliftCommands
            .Include(l => l.IssuedBy)
            .Where(l => l.ForkliftId == request.ForkliftId)
            .AsNoTracking();

            bool desc = (dir?.ToLowerInvariant() == "desc");
            folkliftCommand = (sortBy?.ToLowerInvariant()) switch
            {
                "command" => (desc ? folkliftCommand.OrderByDescending(x => x.Command) : folkliftCommand.OrderBy(x => x.Command)),
                _ => (desc ? folkliftCommand.OrderByDescending(x => x.CreatedAt) : folkliftCommand.OrderBy(x => x.CreatedAt))
            };

            var total = await folkliftCommand.CountAsync(ct);
            var list = await folkliftCommand.Skip((page - 1) * size).Take(size).ToListAsync(ct);
            var items = list.Select(l => new FolkliftCommandDto(
            l.Id,
            l.ForkliftId,
            l.Command,
            JsonSerializer.Deserialize<List<ParsedActionDto>>(l.ParsedActionsJson) ?? new List<ParsedActionDto>(),
            l.CreatedAt,
            l.IssuedBy?.Username ?? "unknown"
            )).ToList();

            return new PagedResult<FolkliftCommandDto> { Page = page, Size = size, TotalItems = total, Items = items };
        }
    }
}
