﻿using System;
using System.Collections.Generic;

namespace RecordLabel.Data.ok
{
    public interface IRecursiveEntityUpdater : IEntityUpdater
    {
        TEntity UpdateEntity<TEntity>(TEntity model, IRecursiveEntityUpdater updater) where TEntity : class;
    }
}
