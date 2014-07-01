namespace IORPromoteTool.IORPromoteEntities
{
    using System;
    using System.Data.Entity;
    using System.Data.EntityClient;

    /// <summary>
    /// Partial class for DevAnswersEntities that allows for a constructor that takes an EntityConnection argument.
    /// </summary>
    public partial class DevPromoteEntities : DbContext, IDevPromoteEntities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DevPromotesEntities"/> class.
        /// </summary>
        /// <param name="connection">The entity connection.</param>
        public DevPromoteEntities(EntityConnection connection)
            : base(connection, false)
        {
        }

        /// <summary>
        /// This is the same as the DbContext Set only it returns IDbSet instead of DbSet.
        /// Returns a non-generic DbSet instance for access to entities of the given type in the context, the ObjectStateManager, and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns> A set for the given entity type. </returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}