using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AndrewD.EntityPlus.Persistence
{
    public class EntityUpdater : EntityUpdaterBase
    {
        protected DbContextReflector Reflector { get; }
        protected ICollectionMerger CollectionMerger { get; }

        public EntityUpdater(DbContext dbContext, DbContextReflector reflector, IEntityUpdater scalarPropertyUpdater, ICollectionMerger collectionMerger)
            : base(dbContext, scalarPropertyUpdater)
        {
            Reflector = reflector;
            CollectionMerger = collectionMerger;
        }

        protected override void UpdateAllNavigationProperties<TEntity>(TEntity updatedModel, TEntity model, IRecursiveEntityUpdater entityUpdater, bool isNew)
        {
            EntityPropertyInfo[] collectionProperties = Reflector.GetCollectionNavigationProperties(typeof(TEntity)); // make it work like <TModel>

            var collectionUpdater = new ReflectingGenericCollectionPropertyUpdater<TEntity>(new CollectionPropertyUpdater<TEntity>(DbContext, updatedModel, CollectionMerger));
            foreach (var property in collectionProperties)
            {
                collectionUpdater.UpdateCollectionProperty(property, Reflector.GetKeyProperties<TEntity>(), model, isNew, entityUpdater);
            }
        }
    }
}
