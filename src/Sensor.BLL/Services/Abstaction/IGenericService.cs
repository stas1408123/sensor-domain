using System.Linq.Expressions;

namespace Sensor.BLL.Services.Abstaction
{
    public interface IGenericService<T>
    {
        Task<T> Add(T model);

        Task<T> GetById(Guid id);

        Task<IReadOnlyList<T>> GetAll();

        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate);

        Task<T> Update(T model);

        Task Delete(Guid id);
    }
}
