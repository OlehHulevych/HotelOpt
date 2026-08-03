namespace HotelOpt.Application.DTOs;

public record StaffFairnessDto(Guid StaffId, string StaffName, int WeeklyTaskCount)
{
    public bool IsOverloaded => WeeklyTaskCount>=25;
}


