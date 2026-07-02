using System.Linq.Expressions;
using HotelOpt.Domain.Common;

namespace HotelOpt.Application.Interfaces;

public interface IRepository<T> where T:BaseEntity
{
    public Task<List<T>> GetAll();
    public Task<T> GetById(Guid id);
    public Task<bool> Add(T entity);
    public Task Update(T entity);
    public Task<bool> Delete(Guid id);
    public Task<List<T>> GetByCondition(Expression<Func<T, bool>> predicate);
    Task<(List<T> Items, int TotalCount)> GetAllPaginated(int page, int pageSize);
    Task<(List<T> Items, int TotalCount)> GetByConditionPaginated(Expression<Func<T, bool>> predicate, int page, int pageSize);
    
}