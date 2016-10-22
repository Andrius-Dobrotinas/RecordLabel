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
            IEntityUpdater updater = new EntityUpdater(DbContext, reflector);
            updater.UpdateEntity<TModel>(model, new NavUpdater(DbContext, reflector));            
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }

    public interface IEntityUpdater
    {
        TEntity UpdateEntity<TEntity>(TEntity model, IEntityUpdater updater) where TEntity : class, IHasId;
    }

    public class NavUpdater : EntityUpdater
    {
        public NavUpdater(DbContext dbContext, DbContextReflector reflector) : base(dbContext, reflector)
        {
            
        }

        protected override void UpdateAllNavigationProperties<TEntity>(TEntity updatedModel, TEntity model, IEntityUpdater entityUpdater, bool modelIsNew)
        {
            EntityPropertyInfo[] navigationProperties = reflector.GetDependentNavigationProperties(typeof(TEntity));

            var modelUpdater = new GenericModelUpdater<TEntity>();
            foreach (var property in navigationProperties)
            {
                modelUpdater.UpdateProperty(property, model, entityUpdater);

                /* TODO: cover scenarios where navigation property has a corresponding foreign key (Id) property.
                 * Make sure updates work well in those cases because sometimes navigation property might not be loaded (== null)
                 * but the foreign key property is non-default. In such cases, navigation property of the target model should
                 * not be set to null. */
            }
        }
    }

    public class EntityUpdater : IEntityUpdater
    {
        protected DbContext DbContext { get; }
        protected DbContextReflector reflector { get; }

        public EntityUpdater(DbContext dbContext, DbContextReflector reflector)
        {
            this.DbContext = dbContext;
            this.reflector = reflector;
        }

        public TEntity UpdateEntity<TEntity>(TEntity model, IEntityUpdater updater)
            where TEntity : class, IHasId
        {
            DbContextReflector reflector = new DbContextReflector(DbContext, "RecordLabel.Data.Models");
            ScalarPropertyUpdater scalarPropertyUpdater = new ScalarPropertyUpdater(DbContext);

            TEntity updatedModel = scalarPropertyUpdater.UpdateModelProperties<TEntity>(model);
            
            UpdateAllNavigationProperties(updatedModel, model, updater, !(updatedModel == model));

            return updatedModel;
        }

        protected virtual void UpdateAllNavigationProperties<TEntity>(TEntity updatedModel, TEntity model, IEntityUpdater entityUpdater, bool isNew)
            where TEntity : class, IHasId
        {
            EntityPropertyInfo[] collectionProperties = reflector.GetCollectionNavigationProperties(typeof(TEntity)); // make it work like <TModel>

            var collectionUpdater = new GenericCollectionUpdater<TEntity>(new CollectionUpdater<TEntity>(DbContext, updatedModel));
            foreach (var property in collectionProperties)
            {
                // TODO: implement updates for LocalizedString collection
                if (property.PropertyName.Contains("Localized")) continue;

                collectionUpdater.UpdateCollectionProperty(property, model, entityUpdater, isNew);
            }
        }
    }

    public delegate Func<DbContext> ModelPropertyUpdaterFactory<TModel>(DbContext dbContext);


    public class ScalarPropertyUpdater
    {
        protected readonly DbContext dbContext;

        public ScalarPropertyUpdater(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public TModel UpdateModelProperties<TModel>(TModel model) where TModel : class, IHasId
        {
            // If adding new entity
            if (model.Id == default(int))
            {
                return dbContext.Set<TModel>().Add(model);
                // TODO: if we add new entity but assign, like, existing metadata... do that else if anyway.
            }
            else
            {
                /* Beware of searching for entities that have base entities and whose Ids are bad. In such situation, if
                 * there is an other entity that derives from the base entity with that Id, Find will return it, and then
                 * will throw an invalid case exception because it will try to cast it to TModel.
                 * In other words, it won't say it could not find TModel with the supplied Id, it will find a "sibling"
                 * entity with that Id (if it exists) and try to cast it. */
                TModel originalModel = dbContext.Set<TModel>().Find(model.Id);
                dbContext.Entry(originalModel).CurrentValues.SetValues(model);
                return originalModel;
            }
        }
    }

   

    public class ReleaseRepository : Repository<ReleaseContext, Release>
    {
        public ReleaseRepository(ReleaseContext context) : base(context)
        {

        }
    }
}
