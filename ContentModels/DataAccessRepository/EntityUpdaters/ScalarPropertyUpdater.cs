using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RecordLabel.Data.ok
{
    public class ScalarPropertyUpdater : IEntityUpdater
    {
        protected DbContext DbContext { get; }
        protected DbContextReflector Reflector { get; }

        public ScalarPropertyUpdater(DbContext dbContext, DbContextReflector reflector)
        {
            this.DbContext = dbContext;
            this.Reflector = reflector;
        }

        public TModel UpdateEntity<TModel>(TModel model) where TModel : class, IHasId
        {
            var keys = Reflector.GetKeyProperties<TModel>()
                .Select(x => new
                {
                    CurrentValue = x.PropertyInfo.GetValue(model),
                    DefaultValue = x.DefaultValue
                });

            // If adding new entity
            // TODO: review this. See if there are scenarios where this wouldn't work
            if (keys.Any(x => x.CurrentValue.Equals(x.DefaultValue)))
            {
                return DbContext.Set<TModel>().Add(model);
                // TODO: if we add new entity but assign, like, existing metadata... do that else if anyway.
            }
            else
            {
                object[] keyValues = keys.Select(x => x.CurrentValue).ToArray();

                /* Beware of searching for entities that have base entities and whose Ids are bad. In such situation, if
                 * there is an other entity that derives from the base entity with that Id, Find will return it, and then
                 * will throw an invalid case exception because it will try to cast it to TModel.
                 * In other words, it won't say it could not find TModel with the supplied Id, it will find a "sibling"
                 * entity with that Id (if it exists) and try to cast it. */
                TModel originalModel = DbContext.Set<TModel>().Find(keyValues);
                if (originalModel == null)
                    throw new KeyNotFoundException($"Entity of type \"{typeof(TModel)}\" with a key value \"{keyValues}\" not found in the context");

                DbContext.Entry(originalModel).CurrentValues.SetValues(model);
                return originalModel;
            }
        }
    }
}
