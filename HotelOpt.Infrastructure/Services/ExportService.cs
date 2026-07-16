using System.Globalization;
using CsvHelper;
using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Entities;

namespace HotelOpt.Infrastructure.Services;

public class ExportService:IExportService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRepository<HouseKeepingTask> _taskRepository;
    private readonly IRepository<Invoice> _invoiceRepository;

    public ExportService(IBookingRepository bookingRepository, IRepository<HouseKeepingTask> taskRepository, IRepository<Invoice> invoiceRepository)
    {
        _bookingRepository = bookingRepository;
        _taskRepository = taskRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<byte[]> ExportBookingsAsync(Guid propertyId)
    {
        var bookings = await _bookingRepository.GetByCondition((b=>b.PropertyId==propertyId));
        var records = bookings.Select(b => new
        {
            b.Id,
            b.RoomId,
            CheckIn = b.CheckInDate,
            CheckOut = b.CheckOutDate,
            Status = b.Status.ToString()
        });
        using var ms = new MemoryStream();
        await using var writer = new StreamWriter(ms);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(records);
        await writer.FlushAsync();
        return ms.ToArray();
    }

    public async Task<byte[]> ExportInvoicesAsync()
    {
        var invoices = await _invoiceRepository.GetAll();
        var records = invoices.Select(i => new
        {
            i.Id,
            i.RoomId,
            PricePerNight = i.PricePerNight,
            Nights = i.Nights,
            Name = i.GuestName,
            Amount = i.TotalAmount,
            CheckIn = i.CheckInDate,
            CheckOut = i.CheckOutDate,
            i.CreatedAt
        });
        using var ms = new MemoryStream();
        await using var writer = new StreamWriter(ms);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(records);
        await writer.FlushAsync();
        return ms.ToArray();
    }

    public async Task<byte[]> ExportTasksAsync(Guid propertyId)
    {
        var tasks = await _taskRepository.GetByCondition(t=>t.PropertyId==propertyId);
        var records = tasks.Select(t => new
        {
            t.Id,
            t.RoomId,
            t.Title,
            t.PropertyId,
            t.AssignedToId,
            t.AssignedById,
            t.Status
        });
        using var ms = new MemoryStream();
        await using var writer = new StreamWriter(ms);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(records);
        await writer.FlushAsync();
        return ms.ToArray();
    }
}