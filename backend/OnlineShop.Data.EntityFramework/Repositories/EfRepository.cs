using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EntityFramework.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity> 
        where TEntity : class, IEntity
    {
        protected readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

        public virtual Task<TEntity> GetById(Guid Id, CancellationToken cancellationToken)
            => Entities.FirstAsync(it => it.Id == Id, cancellationToken);

        public virtual async Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken)
            => await Entities.ToListAsync(cancellationToken);

        public virtual async Task Add(TEntity entity, CancellationToken cancellationToken)
        {
            await Entities.AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task Update(TEntity TEntity, CancellationToken cancellationToken)
        {
            _dbContext.Entry(TEntity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
