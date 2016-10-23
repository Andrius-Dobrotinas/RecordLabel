﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RecordLabel.Data.ok
{
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
}