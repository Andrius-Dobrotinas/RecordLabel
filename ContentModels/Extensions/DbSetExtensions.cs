using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RecordLabel.Data.Models.Extensions
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// Retrieves a collection of items from the dbSet by their IDs
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="ids"></param>
        public static IList<TModel> RetrieveItems<TModel>(this DbSet<TModel> dbSet, IList<int> ids) where TModel : class, IHasId
        {
            if (ids == null)
            {
                return new TModel[0];
            }

            return dbSet.Where(r => ids.Contains(r.Id)).ToArray();
        }
    }
}
