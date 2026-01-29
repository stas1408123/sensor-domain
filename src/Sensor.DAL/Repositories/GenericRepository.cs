using Microsoft.EntityFrameworkCore;
using Sensor.DAL.Repositories.Abstarction;
using System.Linq.Expressions;

namespace Sensor.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        SensorDbContext _context;
        DbSet<TEntity> _dbSet;

        public GenericRepository(SensorDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async virtual Task<TEntity> Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity is not null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async virtual Task<IReadOnlyList<TEntity>> GetAll()
        {
            Console.WriteLine("________________________________________________________");
            Console.WriteLine(_context.ContextId);
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async virtual Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
