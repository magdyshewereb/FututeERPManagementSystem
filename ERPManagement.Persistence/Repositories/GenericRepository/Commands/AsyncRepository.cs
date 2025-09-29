using ERPManagement.Persistence;
using Hr.Application.Interfaces.GenericRepository.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Hr.Persistence.Contexts.GenericRepository.Repository
{
    public sealed partial class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _entity;

        public BaseRepository(ApplicationDbContext context)
        {
            _dbContext = context;
            _entity = _dbContext.Set<T>();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        public async Task RollbackTransactionAsync()
            => await _dbContext.Database.RollbackTransactionAsync();

        public async Task CommitTransactionAsync()
            => await _dbContext.Database.CommitTransactionAsync();

        public async Task AddAsync(T entity)
            => await _entity.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _entity.AddRangeAsync(entities);

        public async Task DeleteAsync(long id)
        {
            var entity = await _entity.FindAsync(id);
            if (entity != null)
                await DeleteAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _entity.FindAsync(id);
            if (entity != null)
                await DeleteAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _entity.Attach(entity);

            _entity.Remove(entity);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
            => _entity.RemoveRange(entities);

        public async Task UpdateAsync(T entity)
        {
            _entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            var collections = _dbContext.Entry(entity).Collections;
            foreach (var collection in collections)
            {
                if (collection.CurrentValue is IEnumerable<object> items)
                {
                    foreach (var item in items)
                    {
                        var idProp = item.GetType().GetProperty("ID");
                        if (idProp != null)
                        {
                            var idValue = Convert.ToInt32(idProp.GetValue(item));
                            _dbContext.Entry(item).State = idValue > 0 ? EntityState.Modified : EntityState.Added;
                        }
                    }
                }
            }

            _entity.Update(entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
            => _entity.UpdateRange(entities);

        public async Task<T?> GetByIdAsync(Guid id)
            => await _entity.FindAsync(id);

        public async Task<IReadOnlyList<T>> ListAllAsync()
            => await _entity.ToListAsync();
    }
}
