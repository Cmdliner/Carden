namespace Carden.Api.Dtos;

public record UpdateItemDto();

public record UpdateItemPriorityDto
{
    public uint NewPriority { get; set; }
}