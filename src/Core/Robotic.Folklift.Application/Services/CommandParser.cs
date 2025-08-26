using FluentValidation;
using Robotic.Folklift.Application.Interfaces;
using System.Text.RegularExpressions;
using Robotic.Folklift.Application.Dtos;

namespace Robotic.Folklift.Application.Services
{
    public class CommandParser : ICommandParser
    {
        private static readonly Regex FolkliftCommand = new(@"([FBLR])(\d+)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        public List<ParsedActionDto> Parse(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return new();
            var normalized = command.Trim().ToUpperInvariant();
            var matches = FolkliftCommand.Matches(normalized);
            if (matches.Count == 0 || string.Concat(matches.Select(m => m.Value)) != normalized)
                throw new ValidationException("Invalid command format. Example: F10R90L90B5");

            var actions = new List<ParsedActionDto>();
            foreach (Match m in matches)
            {
                var code = m.Groups[1].Value;
                var value = int.Parse(m.Groups[2].Value);
                switch (code)
                {
                    case "F": actions.Add(new("Forward", value, "metres")); break;
                    case "B": actions.Add(new("Backward", value, "metres")); break;
                    case "L": ValidateDegrees(value); actions.Add(new("Left", value, "degrees")); break;
                    case "R": ValidateDegrees(value); actions.Add(new("Right", value, "degrees")); break;
                    default: throw new ValidationException($"Unsupported code '{code}'.");
                }
            }
            return actions;
        }

        private static void ValidateDegrees(int d)
        {
            if (d < 0 || d > 360) throw new ValidationException("Degrees must be between 0 and 360");
            if (d % 90 != 0) throw new ValidationException("Degrees must be a multiple of 90");
        }
    }
}
