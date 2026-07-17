using System.Linq.Expressions;
using HotelOpt.Application.Exceptions;
using HotelOpt.Application.Interfaces;
using HotelOpt.Domain.Common;
using HotelOpt.Domain.Entities;
using HotelOpt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelOpt.Infrastructure.Repositories;

public class TaskTemplateRepository : ITaskTemplateRepository
{
    private readonly AppDbContext _context;

    public TaskTemplateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskTemplate>> GetByPropertyWithItemsAsync(Guid propertyId)
    {
        return await _context.TaskTemplates
            .Include(t => t.Items)
            .Where(t => t.PropertyId == propertyId)
            .ToListAsync();
    }

    public async Task<TaskTemplate> GetByIdWithItemsAsync(Guid id)
    {
        var template = await _context.TaskTemplates
            .Include(t => t.Items)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (template == null) throw new NotFoundException($"TaskTemplate {id} was not found");
        return template;
    }

    public async Task<List<TaskTemplate>> GetAll()
        => await _context.TaskTemplates.Include(t => t.Items).ToListAsync();

    public async Task<TaskTemplate> GetById(Guid id)
    {
        var template = await _context.TaskTemplates.Include(t => t.Items).FirstOrDefaultAsync(t => t.Id == id);
        if (template == null) throw new NotFoundException($"TaskTemplate {id} was not found");
        return template;
    }

    public async Task<bool> Add(TaskTemplate entity)
    {
        await _context.TaskTemplates.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task Update(TaskTemplate entity)
    {
        _context.TaskTemplates.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await _context.TaskTemplates.FindAsync(id);
        if (entity == null) return false;
        entity.Delete();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<TaskTemplate>> GetByCondition(Expression<Func<TaskTemplate, bool>> predicate)
        => await _context.TaskTemplates.Include(t => t.Items).Where(predicate).ToListAsync();

    public async Task<(List<TaskTemplate> Items, int TotalCount)> GetAllPaginated(int page, int pageSize)
    {
        var query = _context.TaskTemplates.Include(t => t.Items);
        int total = await query.CountAsync();
        var list = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (list, total);
    }

    public async Task<(List<TaskTemplate> Items, int TotalCount)> GetByConditionPaginated(
        Expression<Func<TaskTemplate, bool>> predicate, int page, int pageSize,
        Expression<Func<TaskTemplate, object>>? orderBy = null, bool descending = false)
    {
        var query = _context.TaskTemplates.Include(t => t.Items).Where(predicate);
        if (orderBy != null)
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        int total = await query.CountAsync();
        var list = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (list, total);
    }

    public async Task<TaskTemplate?> GetByIdWithIncludes(Guid id, params Expression<Func<TaskTemplate, object>>[] includes)
    {
        IQueryable<TaskTemplate> query = _context.TaskTemplates.Include(t => t.Items);
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<TaskTemplate?> GetSingleByCondition(Expression<Func<TaskTemplate, bool>> predicate)
        => await _context.TaskTemplates.Include(t => t.Items).FirstOrDefaultAsync(predicate);
}
