
using System.Collections.Generic;
using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace ImVehicleCore.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(long id);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task<List<T>> ListRangeAsync(int start, int count);
        Task<List<T>> ListRangeAsync(ISpecification<T> spec, int start, int count);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
