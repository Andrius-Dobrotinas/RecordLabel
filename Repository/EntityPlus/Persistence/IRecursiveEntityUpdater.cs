using System;
using System.Collections.Generic;

namespace AndrewD.EntityPlus.Persistence
{
    public interface IRecursiveEntityUpdater : IEntityUpdater
    {
        TEntity UpdateEntity<TEntity>(TEntity model, IRecursiveEntityUpdater updater) where TEntity : class;
    }
}
