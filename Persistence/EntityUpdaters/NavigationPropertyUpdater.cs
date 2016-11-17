using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AndrewD.EntityPlus.Persistence
{
    /// <summary>
    /// Updates entity scalar and navigation properties
    /// </summary>
    public class NavigationPropertyUpdater : EntityUpdaterBase
    {
        protected IDbContextReflector Reflector { get; }

        public NavigationPropertyUpdater(DbContext dbContext, IEntityUpdater scalarEntityUpdater, IDbContextReflector reflector)
            : base(dbContext, scalarEntityUpdater)
        {
            Reflector = reflector;
        }

        protected override void UpdateAllNavigationProperties<TEntity>(TEntity updatedModel, TEntity model, IRecursiveEntityUpdater entityUpdater, bool modelIsNew)
        {
            EntityNavigationPropertyInfo[] navigationProperties = Reflector.GetDependentNavigationProperties(typeof(TEntity));

            var modelUpdater = new ReflectingGenericEntityUpdater<TEntity>();
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
}
