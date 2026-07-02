using HotelOpt.Application.DTOs;

namespace HotelOpt.Application.Interfaces;

public interface IGeminiService
{
    public Task<GeminiInspectionResult> InspectRoom(string photoUrl, string contenType);
}