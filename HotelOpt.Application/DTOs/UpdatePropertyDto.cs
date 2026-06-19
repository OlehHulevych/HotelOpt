namespace HoteOpt.Application.DTOs;

public record UpdatePropertyDto(Guid Id, string Name, string ContactEmail, string PhoneNumber, string Address,int StarRating );