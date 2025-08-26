using FluentValidation;
using MediatR;
using Robotic.Folklift.Application.Dtos;
using Robotic.Folklift.Application.Interfaces;
using Robotic.Folklift.Application.Validations;
using Robotic.Folklift.Domain.Entities;
using System.Text.Json;

namespace Robotic.Folklift.Application.Forklifts.Commands
{
    public record SendCommandCommand(SendCommandRequest Request) : IRequest<List<ParsedActionDto>>;

    public class SendCommandHandler : IRequestHandler<SendCommandCommand, List<ParsedActionDto>>
    {
        private readonly IAppDbContext _db; private readonly ICommandParser _parser; private readonly SendCommandValidator _validator = new();
        public SendCommandHandler(IAppDbContext db, ICommandParser parser) { _db = db; _parser = parser; }

        public async Task<List<ParsedActionDto>> Handle(SendCommandCommand cmd, CancellationToken ct)
        {
            var req = cmd.Request;
            var val = await _validator.ValidateAsync(req, ct);
            if (!val.IsValid) throw new ValidationException(string.Join("; ", val.Errors.Select(e => e.ErrorMessage)));

            var forklift = await _db.Forklifts.FindAsync(new object?[] { req.ForkliftId }, ct);
            if (forklift == null) throw new KeyNotFoundException("Forklift not found");

            var actions = _parser.Parse(req.Command);
            var log = new FolkliftCommand
            {
                ForkliftId = forklift.Id,
                IssuedByUserId = req.IssuedByUserId,
                Command = req.Command,
                ParsedActionsJson = JsonSerializer.Serialize(actions)
            };
            _db.FolkliftCommands.Add(log);
            await _db.SaveChangesAsync(ct);
            return actions;
        }
    }
}
