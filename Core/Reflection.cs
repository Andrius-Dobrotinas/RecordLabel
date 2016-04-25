using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace RecordLabel.Reflection
{
    public static class Reflection
    {
        /// <summary>
        /// Returns MemberInfo for a given model property  
        /// </summary>
        /// <typeparam name="TSource">Source type that contains the property</typeparam>
        /// <typeparam name="TProperty">Property to return member info for</typeparam>
        /// <param name="property">Expression that returns source type's property</param>
        /// <returns></returns>
        public static MemberInfo GetMemberInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> property)
        {
            MemberExpression mex = (MemberExpression)property.Body;
            return mex.Member;
        }

        /// <summary>
        /// Returns a list of all properties that lead from TSource to TProperty and checks if they belong to TSource type (throwing exception if not)
        /// </summary>
        /// <typeparam name="TSource">Source type that contains the property</typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property"></param>
        /// <exception cref="ArgumentException">TProperty does not belong to TSource</exception>
        /// <returns></returns>
        public static IList<MemberInfo> GetMemberInfoRecursive<TSource, TProperty>(Expression<Func<TSource, TProperty>> property)
        {
            MemberExpression mex = (MemberExpression)property.Body;
            IList<MemberInfo> result = GetPropertyTree(mex);

            if (result.Count > 0 && result[0].ReflectedType.IsAssignableFrom(typeof(TSource)) == false)  //Make sure the property belongs to the Source type
            {
                throw new ArgumentException($"The property {property.Name} does not belong to the source type {typeof(TSource)}");
            }
            return result;
        }

        /// <summary>
        /// Returns an ordered array of all properties that lead from source to destination member
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertyTree(MemberExpression memberExpression)
        {
            MemberExpression mex = memberExpression;
            List<PropertyInfo> list = new List<PropertyInfo>();
            while (mex != null)
            {
                list.Add(mex.Member as PropertyInfo);
                mex = mex.Expression as MemberExpression;
            }
            list.Reverse();
            return list.ToArray();
        }

        /// <summary>
        /// Casts the supplied property to PropertyInfo and returns its value in the source model
        /// </summary>
        /// <typeparam name="TSource">Type of source object</typeparam>
        /// <param name="sourceModel">Model whose property value to retrieve</param>
        /// <param name="property">Property whose value to retrieve</param>
        /// <returns></returns>
        public static object GetPropertyValue<TSource>(TSource sourceModel, MemberInfo property)
        {
            return ((PropertyInfo)property).GetValue(sourceModel);
        }
    }
}
