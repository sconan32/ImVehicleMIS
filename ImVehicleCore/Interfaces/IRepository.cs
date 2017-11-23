
using System.Collections.Generic;
using ImVehicleCore.Data;

namespace ImVehicleCore.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(long id);
        T GetSingleBySpec(ISpecification<T> spec);
        IEnumerable<T> ListAll();
        IEnumerable<T> List(ISpecification<T> spec);

        IEnumerable<T> ListRange(int start, int count);
        IEnumerable<T> ListRange(ISpecification<T> spec, int start, int count);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
