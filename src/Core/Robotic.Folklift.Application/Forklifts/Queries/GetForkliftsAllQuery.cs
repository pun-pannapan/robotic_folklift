using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Robotic.Folklift.Application.Dtos;
using Robotic.Folklift.Application.Interfaces;

namespace Robotic.Folklift.Application.Forklifts.Queries
{
    public record GetForkliftsAllQuery(PageQuery Query) : IRequest<PagedResult<ForkliftDto>>;

    public class GetForkliftsAllHandler : IRequestHandler<GetForkliftsAllQuery, PagedResult<ForkliftDto>>
    {
        private readonly IAppDbContext _db; private readonly IMapper _mapper;
        public GetForkliftsAllHandler(IAppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

        public async Task<PagedResult<ForkliftDto>> Handle(GetForkliftsAllQuery request, CancellationToken ct)
        {
            var (page, size, sortBy, dir) = request.Query;
            var q = _db.Forklifts.AsNoTracking();


            // sorting
            bool desc = (dir?.ToLowerInvariant() == "desc");
            q = (sortBy?.ToLowerInvariant()) switch
            {
                "modelnumber" => (desc ? q.OrderByDescending(x => x.ModelNumber) : q.OrderBy(x => x.ModelNumber)),
                "manufacturingdate" => (desc ? q.OrderByDescending(x => x.ManufacturingDate) : q.OrderBy(x => x.ManufacturingDate)),
                _ => (desc ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id))
            };

            var total = await q.CountAsync(ct);
            var items = await q.Skip((page - 1) * size).Take(size)
            .ProjectTo<ForkliftDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

            return new PagedResult<ForkliftDto> { Page = page, Size = size, TotalItems = total, Items = items };
        }
    }
}
