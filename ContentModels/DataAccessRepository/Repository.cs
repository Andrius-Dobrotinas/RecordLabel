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
            IEntityUpdater scalarUpdater = new ScalarPropertyUpdater(DbContext);
            IRecursiveEntityUpdater updater = new EntityUpdater(DbContext, scalarUpdater, reflector);
            IRecursiveEntityUpdater navUpdater = new NavUpdater(DbContext, scalarUpdater, reflector);
            updater.UpdateEntity<TModel>(model, navUpdater);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }

    public interface IEntityUpdater
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns>Returns an updated entity if an existing entity has been updated, or originally supplied entity if the entity is new</returns>
        TEntity UpdateEntity<TEntity>(TEntity model) where TEntity : class, IHasId;
    }

    public interface IRecursiveEntityUpdater : IEntityUpdater
    {
        TEntity UpdateEntity<TEntity>(TEntity model, IRecursiveEntityUpdater updater) where TEntity : class, IHasId;
    }

    public class NavUpdater : EntityUpdaterBase
    {
        protected DbContextReflector Reflector { get; }

        public NavUpdater(DbContext dbContext, IEntityUpdater scalarEntityUpdater, DbContextReflector reflector)
            : base(dbContext, scalarEntityUpdater)
        {
            this.Reflector = reflector;
        }

        protected override void UpdateAllNavigationProperties<TEntity>(TEntity updatedModel, TEntity model, IRecursiveEntityUpdater entityUpdater, bool modelIsNew)
        {
            EntityPropertyInfo[] navigationProperties = Reflector.GetDependentNavigationProperties(typeof(TEntity));

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

    public class EntityUpdater : EntityUpdaterBase
    {
        protected DbContextReflector Reflector { get; }

        public EntityUpdater(DbContext dbContext, IEntityUpdater scalarPropertyUpdater, DbContextReflector reflector)
            : base(dbContext, scalarPropertyUpdater)
        {
            this.Reflector = reflector;
        }

        protected override void UpdateAllNavigationProperties<TEntity>(TEntity updatedModel, TEntity model, IRecursiveEntityUpdater entityUpdater, bool isNew)
        {
            EntityPropertyInfo[] collectionProperties = Reflector.GetCollectionNavigationProperties(typeof(TEntity)); // make it work like <TModel>

            var collectionUpdater = new GenericCollectionUpdater<TEntity>(new CollectionUpdater<TEntity>(DbContext, updatedModel));
            foreach (var property in collectionProperties)
            {
                // TODO: implement updates for LocalizedString collection
                if (property.PropertyName.Contains("Localized")) continue;

                collectionUpdater.UpdateCollectionProperty(property, model, entityUpdater, isNew);
            }
        }
    }

    public abstract class EntityUpdaterBase : IRecursiveEntityUpdater
    {
        protected DbContext DbContext { get; }
        protected IEntityUpdater ScalarPropertyUpdater { get; }

        public EntityUpdaterBase(DbContext dbContext, IEntityUpdater scalarPropertyUpdater)
        {
            this.DbContext = dbContext;
            this.ScalarPropertyUpdater = scalarPropertyUpdater;
        }

        public TEntity UpdateEntity<TEntity>(TEntity model, IRecursiveEntityUpdater updater)
            where TEntity : class, IHasId
        {
            // Update scalar properties
            TEntity updatedModel = UpdateEntity<TEntity>(model);
            
            UpdateAllNavigationProperties(updatedModel, model, updater, updatedModel == model);

            return updatedModel;
        }

        public TEntity UpdateEntity<TEntity>(TEntity model)
            where TEntity : class, IHasId
        {
            return ScalarPropertyUpdater.UpdateEntity<TEntity>(model);
        }

        protected abstract void UpdateAllNavigationProperties<TEntity>(TEntity updatedModel, TEntity model, IRecursiveEntityUpdater entityUpdater, bool isNew)
            where TEntity : class, IHasId;
    }

    public delegate Func<DbContext> ModelPropertyUpdaterFactory<TModel>(DbContext dbContext);


    public class ScalarPropertyUpdater : IEntityUpdater
    {
        protected readonly DbContext dbContext;

        public ScalarPropertyUpdater(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TModel UpdateEntity<TModel>(TModel model) where TModel : class, IHasId
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
