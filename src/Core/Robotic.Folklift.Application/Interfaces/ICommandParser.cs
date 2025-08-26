using Robotic.Folklift.Application.Dtos;

namespace Robotic.Folklift.Application.Interfaces
{
    public interface ICommandParser
    {
        List<ParsedActionDto> Parse(string command);
    }
}
