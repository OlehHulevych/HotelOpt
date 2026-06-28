namespace HotelOpt.Application.DTOs;

public record StaffFairnessDto(Guid StaffId, int WeeklyTaskCount)
{
    public bool IsOverloaded => WeeklyTaskCount>=25;
}


