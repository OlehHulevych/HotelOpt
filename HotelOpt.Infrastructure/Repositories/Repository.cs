using HotelOpt.Domain.Common;
using HotelOpt.Infrastructure.Data;
using HoteOpt.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelOpt.Infrastructure.Repositories;

public class Repository<T>:IRepository<T> where T:BaseEntity
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<T>> GetAll()
    {
        List<T> list = await _context.Set<T>().ToListAsync();
        return list;
    }

    public async Task<T> GetById(Guid id)
    {
        T? entity = await _context.Set<T>().FindAsync(id);
        if (entity == null) throw new Exception("Failed to find your entity");
        return entity;

    }

    public async Task<bool> Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity); 
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(Guid id)
    {
        await _context.Set<T>().Where(e=>e.Id ==id).ExecuteDeleteAsync();
        return true;
    }
}