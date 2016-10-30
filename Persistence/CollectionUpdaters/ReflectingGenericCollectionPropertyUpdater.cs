using System;
using System.Collections.Generic;
using System.Linq;
using AndrewD.EntityPlus.Reflection;

namespace AndrewD.EntityPlus.Persistence
{
    public class ReflectingGenericCollectionPropertyUpdater<TModel> where TModel : class
    {
        public ICollectionPropertyUpdater<TModel> CollectionPropertyUpdater { get; }

        public ReflectingGenericCollectionPropertyUpdater(ICollectionPropertyUpdater<TModel> collectionPropertyUpdater)
        {
            CollectionPropertyUpdater = collectionPropertyUpdater;
        }

        // TODO: might add a method with lambda expression that selects model property
        public void UpdateCollectionProperty(IDbContextReflector reflector, EntityNavigationPropertyInfo property, TModel newModel, bool isNew, IEntityUpdater entityUpdater)
        {
            var genericArguments = property.PropertyInfo.PropertyType.GetGenericArguments();

            var genericType = typeof(ICollectionPropertyUpdater<>).MakeGenericType(typeof(TModel));

            GenericMethodInvoker.InvokeGenericMethod(genericType, nameof(ICollectionPropertyUpdater<TModel>.UpdateCollection),
                genericArguments,
                GenericMethodInvoker.DefaultPublicInstanceBindingFlags,
                new object[] { reflector, property, newModel, isNew, entityUpdater },
                this.CollectionPropertyUpdater);
        }
    }
}
