using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Data.ok
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Copies value of the specified properties from the source object to the target object
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="target">Object to copy property value to</param>
        /// <param name="source">Object to copy property value from</param>
        /// <param name="properties">Property whose value to copy</param>
        public static void CopyPropertyValues<TObject>(this TObject target, TObject source, PropertyInfo property)
            where TObject : class
        {
            property.SetValue(target, property.GetValue(source));
        }

        /// <summary>
        /// Copies each values of specified properties from the source object to the target object
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="target">Object to copy property values to</param>
        /// <param name="source">Object to copy property values from</param>
        /// <param name="properties">Properties whose values to copy</param>
        public static void CopyPropertyValues<TObject>(this TObject target, TObject source, params PropertyInfo[] properties)
            where TObject : class
        {
            foreach (var property in properties)
            {
                property.SetValue(target, property.GetValue(source));
            }
        }
    }
}
