namespace Robotic.Folklift.Application.Dtos
{
    public record ParsedActionDto(string Action, int Value, string Unit);
    public record SendCommandRequest(int ForkliftId, string Command, int IssuedByUserId);
    public record FolkliftCommandDto(
    int Id,
    int ForkliftId,
    string Command,
    List<ParsedActionDto> ParsedActions,
    DateTime CreatedAt,
    string IssuedBy
    );
}
