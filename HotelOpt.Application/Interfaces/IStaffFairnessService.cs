using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IStaffFairnessService
{
    public Task<List<StaffFairnessDto>> GetStaffFairness();
}