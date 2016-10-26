using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RecordLabel.Data.Models;
using System.Reflection;
using RecordLabel.Data.ok;
using RecordLabel.Data.Context;

namespace RecordLabel.Data.ok
{
    public abstract class Repository<TContext, TModel>
        where TContext : DbContext
        where TModel : EntityBase
    {
        protected TContext DbContext { get; set; }

        protected Repository(TContext context)
        {
            DbContext = context;
        }

        public virtual TModel GetModel(int id)
        {
            return DbContext.Set<TModel>().Find(id);
        }

        public virtual void SaveModel(TModel model)
        {
            var reflector = new DbContextReflector(DbContext, "RecordLabel.Data.Models");
            IEntityUpdater scalarUpdater = new ScalarPropertyUpdater(DbContext, reflector);
            ICollectionMerger collectionMerger = new CollectionMerger();
            IRecursiveEntityUpdater updater = new EntityUpdater(DbContext, scalarUpdater, reflector, collectionMerger);
            IRecursiveEntityUpdater navUpdater = new NavUpdater(DbContext, scalarUpdater, reflector);
            updater.UpdateEntity<TModel>(model, navUpdater);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }

    public delegate Func<DbContext> ModelPropertyUpdaterFactory<TModel>(DbContext dbContext);

    public class ReleaseRepository : Repository<ReleaseContext, Release>
    {
        public ReleaseRepository(ReleaseContext context) : base(context)
        {

        }
    }
}
