using HotelOpt.Application.DTOs;

namespace HoteOpt.Application.Interfaces;

public interface IStaffFairnessService
{
    public Task<List<StaffFairnessDto>> GetStaffFairness();
}