using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Hr.Persistence.Contexts.GenericRepository.Repository
{
    public sealed partial class BaseRepository<T> where T : class
    {
        public IDbContextTransaction BeginTransaction()
            => _dbContext.Database.BeginTransaction();

        public void RollbackTransaction()
            => _dbContext.Database.RollbackTransaction();

        public void CommitTransaction()
            => _dbContext.Database.CommitTransaction();

        public void Add(T entity)
            => _entity.Add(entity);

        public void AddRange(IEnumerable<T> entities)
            => _entity.AddRange(entities);

        public void Delete(long id) => DeleteById(id);

        public void Delete(int id) => DeleteById(id);

        private void DeleteById(object id)
        {
            var entity = _dbContext.Find<T>(id);
            if (entity != null)
                Delete(entity);
        }

        public void Delete(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _entity.Attach(entity);

            _entity.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
            => _entity.RemoveRange(entities);

        public void Update(T entity)
        {
            _entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<T> entities)
            => _entity.UpdateRange(entities);

        public void ClearChangeTracker()
            => _dbContext.ChangeTracker.Clear();
    }
}
