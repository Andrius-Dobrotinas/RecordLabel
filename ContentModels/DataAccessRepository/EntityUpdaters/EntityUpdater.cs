using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RecordLabel.Data.ok
{
    public class EntityUpdater : EntityUpdaterBase
    {
        protected DbContextReflector Reflector { get; }
        protected ICollectionMerger CollectionMerger { get; }

        public EntityUpdater(DbContext dbContext, IEntityUpdater scalarPropertyUpdater, DbContextReflector reflector, ICollectionMerger collectionMerger)
            : base(dbContext, scalarPropertyUpdater)
        {
            this.Reflector = reflector;
            this.CollectionMerger = collectionMerger;
        }

        protected override void UpdateAllNavigationProperties<TEntity>(TEntity updatedModel, TEntity model, IRecursiveEntityUpdater entityUpdater, bool isNew)
        {
            EntityPropertyInfo[] collectionProperties = Reflector.GetCollectionNavigationProperties(typeof(TEntity)); // make it work like <TModel>

            var collectionUpdater = new GenericCollectionUpdater<TEntity>(new CollectionUpdater<TEntity>(DbContext, updatedModel, CollectionMerger));
            foreach (var property in collectionProperties)
            {
                // TODO: implement updates for LocalizedString collection
                if (property.PropertyName.Contains("Localized")) continue;

                collectionUpdater.UpdateCollectionProperty(property, model, entityUpdater, isNew);
            }
        }
    }
}
