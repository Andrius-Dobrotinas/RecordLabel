using System;
using System.Collections.Generic;
using System.Linq;
using AndrewD.EntityPlus.Reflection;

namespace AndrewD.EntityPlus.Persistence
{
    public class ReflectingGenericEntityUpdater<TModel> where TModel : class
    {
        public void UpdateProperty(EntityPropertyInfo property, TModel newModel, IRecursiveEntityUpdater entityUpdater)
        {
            GenericMethodInvoker.InvokeGenericMethod(typeof(IRecursiveEntityUpdater), nameof(IRecursiveEntityUpdater.UpdateEntity),
                new Type[] { property.PropertyInfo.PropertyType },
                GenericMethodInvoker.DefaultPublicInstanceBindingFlags,
                new object[] { property.PropertyInfo.GetValue(newModel), entityUpdater },
                entityUpdater);
        }
    }
}
