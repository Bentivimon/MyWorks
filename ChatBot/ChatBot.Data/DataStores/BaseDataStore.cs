using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Data
{
    public class BaseDataStore<TId, TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _db;

        protected readonly DbSet<TEntity> Table;

        protected BaseDataStore(ApplicationDbContext db, DbSet<TEntity> dbSet)
        {
            Table = dbSet;
            _db = db;
        }

        public Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
          => Table.AnyAsync(predicate);

        public bool Delete(TId id)
        {
            var entity = Table.Find(id);
            if (entity != null)
            {
                Table.Remove(entity);
                return true;
            }

            return false;
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                return;

            _db.Entry(entity).State = EntityState.Modified;
        }

        public Task CreateAsync(TEntity entity)
            => _db.AddAsync(entity);

        public async Task<ICollection<TEntity>> GetAsync()
           => await Table.ToListAsync();

        public async Task<ICollection<TEntity>> GetAsync(int skip, int take)
         => await Table.Skip(skip).Take(take).ToListAsync();

        public virtual Task<TEntity> GetAsync(TId id)
            => Table.FindAsync(id);

        public async Task<ICollection<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
            => await Table.Where(expression).ToListAsync();

        public async Task<ICollection<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression, int skip, int take)
           => await Table.Where(expression).Skip(skip).Take(take).ToListAsync();

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => Table.FirstOrDefaultAsync(predicate);

        public Task<long> CountAsync()
            => Table.LongCountAsync();

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
            => Table.LongCountAsync(predicate);

        public Task<int> SaveAsync()
        {
            var changes = _db.ChangeTracker.Entries()
                                           .Count(p =>
                                                  p.State == EntityState.Modified
                                           || p.State == EntityState.Deleted
                                           || p.State == EntityState.Added);

            if (changes == 0)
                return Task.FromResult(1);

            return _db.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetQueryable()
            => Table.AsQueryable();
    }
}
