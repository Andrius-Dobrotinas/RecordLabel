using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using RecordLabel.Data.ok;

namespace RecordLabel.Data
{
    public class GenericModelUpdater<TModel> where TModel : class
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

    public class GenericCollectionUpdater<TModel> where TModel : class
    {
        public ICollectionUpdater<TModel> CollectionUpdater { get; }

        public GenericCollectionUpdater(ICollectionUpdater<TModel> collectionUpdater)
        {
            CollectionUpdater = collectionUpdater;
        }

        // TODO: might add a method with lambda expression that selects model property
        public void UpdateCollectionProperty(EntityPropertyInfo property, IList<EntityKeyPropertyInfo> keyProperties, TModel newModel, bool isNew, IEntityUpdater entityUpdater)
        {
            var genericArguments = property.PropertyInfo.PropertyType.GetGenericArguments();

            var genericType = typeof(ICollectionUpdater<>).MakeGenericType(typeof(TModel));

            GenericMethodInvoker.InvokeGenericMethod(genericType, nameof(ICollectionUpdater<TModel>.UpdateCollection),
                genericArguments,
                GenericMethodInvoker.DefaultPublicInstanceBindingFlags,
                new object[] { property, keyProperties, newModel, isNew, entityUpdater },
                this.CollectionUpdater);
        }
    }

    public class GenericMethodInvoker
    {
        public static BindingFlags DefaultPublicInstanceBindingFlags => BindingFlags.Public | BindingFlags.Instance;

        public static object InvokeGenericMethod(Type classType, Type[] classGenericTypeArguments, string methodName, Type[] methodGenericTypeArguments, BindingFlags methodBindingFlags, object[] methodArguments, object targetObject)
        {
            // TODO: I could probably implement a cache so as not to invoke this every time

            var genericType = classType.MakeGenericType(classGenericTypeArguments);

            return InvokeGenericMethod(genericType, methodName, methodGenericTypeArguments, methodBindingFlags, methodArguments, targetObject);
        }

        public static object InvokeGenericMethod(Type classType, string methodName, Type[] methodGenericTypeArguments, BindingFlags methodBindingFlags, object[] methodArguments, object targetObject)
        {
            var method = classType.GetMethod(methodName, methodBindingFlags);
            if (methodGenericTypeArguments?.Length > 0)
                method = method.MakeGenericMethod(methodGenericTypeArguments);
            return method.Invoke(targetObject, methodArguments);
        }
    }
}
