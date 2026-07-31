using System.Linq.Expressions;
using HotelOpt.Application.Exceptions;
using HotelOpt.Domain.Common;
using HotelOpt.Infrastructure.Data;
using HotelOpt.Application.Interfaces;
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
        if (entity == null) throw new NotFoundException($"{typeof(T).Name} {id} was not found");
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
        T? entity = await _context.Set<T>().FindAsync(id);
        if (entity == null) return false;
        entity.Delete();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<T>> GetByCondition(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    

    public async Task<(List<T> Items, int TotalCount)> GetAllPaginated(int page, int pageSize)
    {
        var query = _context.Set<T>();
        int totalCount = await query.CountAsync();
        List<T> list = await query.Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
        return (list, totalCount);
    }

    public async Task<(List<T> Items, int TotalCount)> GetByConditionPaginated(Expression<Func<T, bool>> predicate, int page, int pageSize, Expression<Func<T, object>>? orderBy = null, bool descending = false)
    {
        var query = _context.Set<T>().Where(predicate);
        if (orderBy != null)
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        int totalCount = await query.CountAsync();
        List<T> list = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (list, totalCount) ;
    }

    public async Task<T?> GetByIdWithIncludes(Guid id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<T?> GetSingleByCondition(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }
}