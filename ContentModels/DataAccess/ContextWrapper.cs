using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace RecordLabel.Content
{
    public class ContextWrapper<TContext> : IContextWrapper<TContext>
        where TContext : DbContext
    {
        public TContext Context { get; }

        public ContextWrapper(TContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Saves changes made to the underlying context
        /// </summary>
        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// Retrieves a set of specified type from the underlying context
        /// </summary>
        public DbSet<T> Set<T>() where T : class
        {
            return Context.Set<T>();
        }

        /// <summary>
        /// Updates model properties with new values
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model">Model to update</param>
        /// <param name="newState">Model that contains new values</param>
        public void UpdateModel<TModel>(TModel model, TModel newState) where TModel : EntityBase
        {
            //TODO: handle deletion of References (get exception now)

            // Select all public properties skipping key property which can't be changed and those that are not mapped
            PropertyInfo[] properties = typeof(TModel).GetProperties()
                .Where(p => p.CanWrite) // have a setter
                .Where(p => !p.IsDefined(typeof(NotMappedAttribute)) && !p.IsDefined(typeof(KeyAttribute)))
                .ToArray();

            // Select all properties that implement ISet<>
            var setPropertyDefinitions = properties
                .Select(prop => new
                {
                    Property = prop,
                    Definition = prop.PropertyType.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISet<>))
                })
                .Where(p => p.Definition != null).ToArray();

            // Update all properties that implement ISet<>
            foreach (var property in setPropertyDefinitions)
            {
                if (property.Definition.GetGenericArguments().Contains(typeof(Image))) continue; // TODO: Images

                InvokeGenericMethod(typeof(SetUpdater<>), property.Definition.GetGenericArguments(), "UpdateSet", new Type[] { typeof(TModel) },
                    BindingFlags.Static | BindingFlags.Public, new object[] { model, newState, property.Property, this }, null);
            }

            PropertyInfo[] setProperties = setPropertyDefinitions.Select(p => p.Property).ToArray();

            var foreignKeyNavigationalProperties = properties
                .Except(setProperties)
                .Where(p => p.IsDefined(typeof(ForeignKeyAttribute)))
                .Select(p =>
                new ForeignKeyProperty
                {
                    Property = p,
                    NavigationalProperty = properties.SingleOrDefault(prop => prop.Name == ((ForeignKeyAttribute)p.GetCustomAttribute(typeof(ForeignKeyAttribute))).Name)
                })
                .ToArray();

            UpdateForeignKeyProperties(
                // Exclude "set" properties because they have been taken care of
                foreignKeyNavigationalProperties.Where(p => setProperties.Contains(p.NavigationalProperty) == false).ToArray(),
                model, newState);

            // Select all foreign key and navigational properties so that they could be excluded next
            PropertyInfo[] foreignKeyProperties = foreignKeyNavigationalProperties.Select(p => p.Property)
                .Union(foreignKeyNavigationalProperties.Where(p => p.NavigationalProperty != null)
                    .Select(p => p.NavigationalProperty)).ToArray();


            // Update collection properties (They are unused now but we still need these properties for further processing)
            PropertyInfo[] collectionProperties = properties.Except(setProperties).Except(foreignKeyProperties)
                .Where(prop => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)
                // TODO: some day write a function that recursively searches for a generic interface definition
                ).ToArray();

            // (UNUSED for the time being)
            /*foreach (var property in collectionProperties)
            {
                var value = property.GetValue(model);
                var newValue = property.GetValue(newState);

                // Update collection values
                InvokeGenericMethod(typeof(CollectionExtensions), "UpdateCollection", property.PropertyType.GetGenericArguments(),
                    BindingFlags.Static | BindingFlags.Public, new object[] { value, newValue }, null);
            }*/


            // Regular properties
            PropertyInfo[] otherProperties = properties.Except(setProperties).Except(foreignKeyProperties).Except(collectionProperties).ToArray();
            foreach (var property in otherProperties)
            {
                property.SetValue(model, property.GetValue(newState));
            }
        }

        /// <summary>
        /// Updates Foreign Key properties and, if required, associated Navigational properties
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="foreignKeyProperties"></param>
        /// <param name="model"></param>
        /// <param name="newState"></param>
        protected virtual void UpdateForeignKeyProperties<TModel>(ForeignKeyProperty[] foreignKeyProperties, TModel model, TModel newState)
        {
            // TODO: handle cases when Foreign key attribute contains a comma-separated list of foreign key names (it's unused in this application)

            foreach (var prop in foreignKeyProperties)
            {
                // Null means that current property is navigational property, not a foreign key property
                if (prop.NavigationalProperty == null)
                {
                    object value = prop.Property.GetValue(model);
                    object newValue = prop.Property.GetValue(newState);

                    // TODO: check this if this actually works
                    // Set new values
                    if (newValue != null && newValue != value)
                        value = newValue;
                }
                else
                {
                    object id = prop.Property.GetValue(model);
                    object newId = prop.Property.GetValue(newState);
                    object value = prop.NavigationalProperty.GetValue(model);
                    object newValue = prop.NavigationalProperty.GetValue(newState);

                    // Set new values
                    prop.Property.SetValue(model, newId);
                    if (newValue != null && newValue != value) // TODO: Should I even check the second condition?
                        prop.NavigationalProperty.SetValue(model, newValue); //TODO: should I attach it to the context?
                }
            }
        }

        protected object InvokeGenericMethod(Type classType, Type[] classGenericTypeArguments, string methodName, Type[] methodGenericTypeArguments, BindingFlags methodBindingFlags, object[] methodArguments, object targetObject)
        {
            // I could probably implement cache so not to invoke this everytime
            var genericType = classType.MakeGenericType(classGenericTypeArguments);

            return InvokeGenericMethod(genericType, methodName, methodGenericTypeArguments, methodBindingFlags, methodArguments, targetObject);
        }

        protected object InvokeGenericMethod(Type classType, string methodName, Type[] methodGenericTypeArguments, BindingFlags methodBindingFlags, object[] methodArguments, object targetObject)
        {
            var method = classType.GetMethod(methodName, methodBindingFlags);
            var genericMethod = method.MakeGenericMethod(methodGenericTypeArguments);
            return genericMethod.Invoke(targetObject, methodArguments);
        }

        protected struct ForeignKeyProperty
        {
            public PropertyInfo Property;
            public PropertyInfo NavigationalProperty;
        }

        
        #region IDisposable Support
        public bool IsDisposed { get; protected set; } = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }

                IsDisposed = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
