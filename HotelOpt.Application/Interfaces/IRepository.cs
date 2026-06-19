using HotelOpt.Domain.Common;

namespace HoteOpt.Application.Interfaces;

public interface IRepository<T> where T:BaseEntity
{
    public Task<List<T>> GetAll();
    public Task<T> GetById(Guid id);
    public Task<bool> Add(T entity);
    public Task Update(T entity);
    public Task<bool> Delete(Guid id);
}