using Microsoft.EntityFrameworkCore;

namespace SELLit.Data.Common.Repositories;

public interface IRepository<TEntity> : IDisposable
    where TEntity : class
{
    IQueryable<TEntity> All();

    IQueryable<TEntity> AllAsNoTracking();

    Task AddAsync(TEntity entity);
    
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    Task<int> SaveChangesAsync();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    DbSet<TEntity> Collection();
}