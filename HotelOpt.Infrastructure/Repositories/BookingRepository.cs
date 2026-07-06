using System.Linq.Expressions;
using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Entities;
using HotelOpt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelOpt.Infrastructure.Repositories;

public class BookingRepository:IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Add(Booking entity)
    {
        await _context.Bookings.AddAsync(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await _context.Bookings.FindAsync(id);
        if (entity == null) return false;

        _context.Bookings.Remove(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<List<Booking>> GetAll()
    {
         List<Booking> list = await _context.Bookings.Include(b=>b.Guests).ToListAsync();
         return list;
    }

    public async Task<(List<Booking> Items, int TotalCount)> GetAllPaginated(int page, int pageSize)
    {
        var query = _context.Bookings.Include(b=>b.Guests).AsQueryable();
        var total = await query.CountAsync();
        var list = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (list, total);
    }

    public async Task<List<Booking>> GetByCondition(Expression<Func<Booking, bool>> predicate)
    {
        return await _context.Bookings.Include(b=>b.Guests).Where(predicate).ToListAsync();
    }

    public async Task<(List<Booking> Items, int TotalCount)> GetByConditionPaginated(Expression<Func<Booking, bool>> predicate, int page, int pageSize)
    {
        var query = _context.Bookings.Include(b=>b.Guests).Where(predicate);
        int totalCount = await query.CountAsync();
        List<Booking> list = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (list, totalCount) ;
    }

    public async Task<Booking> GetById(Guid id)
    {
        Booking? entity = await _context.Bookings.Include(b=>b.Guests).FirstOrDefaultAsync(b=>b.Id==id);
        if (entity == null) throw new Exception("Failed to find your entity");
        return entity;
    }

    public async  Task<(List<Booking> Items, int TotalCount)> GetByPropertyPaginated(Guid propertyId, int page, int pageSize)
    {
        var query =  _context.Bookings.Include(b=>b.Guests).Where(b => b.PropertyId == propertyId).AsQueryable();
        var total = await query.CountAsync();
        var list = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (list, total);
    }

    public async Task Update(Booking entity)
    {
        _context.Bookings.Update(entity); 
        await _context.SaveChangesAsync();
    }
}

