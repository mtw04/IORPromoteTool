namespace IORPromoteTool.IORPromoteEntities
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;

    /// <summary>
    /// Our interface for DbContext to make things more testable.
    /// </summary>
    public interface IDbContext : IDisposable
    {
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry(object entity);
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet Set(Type entityType);
    }
}