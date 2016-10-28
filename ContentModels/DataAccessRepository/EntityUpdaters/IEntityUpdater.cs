using System;
using System.Collections.Generic;

namespace RecordLabel.Data.ok
{
    public interface IEntityUpdater
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns>Returns an updated entity if an existing entity has been updated, or originally supplied entity if the entity is new</returns>
        TEntity UpdateEntity<TEntity>(TEntity model) where TEntity : class;
    }
}
