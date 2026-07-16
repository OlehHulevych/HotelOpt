namespace HotelOpt.Application.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportBookingsAsync(Guid propertyId);
    Task<byte[]> ExportInvoicesAsync();
    Task<byte[]> ExportTasksAsync(Guid propertyId);
}
