using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RecordLabel.Web
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Gets value of the specified type from View Data. If value does not exist, returns default value for the type.
        /// If the value retrieved with the supplied key is of different type than the expected one, an exception will be thrown.
        /// </summary>
        /// <typeparam name="TValue">Expected type of value</typeparam>
        public static TValue GetValue<TValue>(this ViewDataDictionary viewData, string key)
        {
            return GetValue(viewData, key, value => (TValue)value);
        }

        /// <summary>
        /// Gets value of the specified type from View Data. If value does not exist, returns default value for the type.
        /// If the value retrieved with the supplied key is of different type than the expected one, default value for the desired type is returned.
        /// </summary>
        /// <typeparam name="TValue">Expected type of value</typeparam>
        public static TValue GetValueSafe<TValue>(this ViewDataDictionary viewData, string key)
            where TValue : class
        {
            return GetValue(viewData, key, value => value as TValue);
        }

        /// <summary>
        /// Gets a nullable value of the specified type from View Data. If value does not exist, returns default nullable value for the type.
        /// If the value retrieved with the supplied key is of different type than the expected one, default nullable value for the desired type is returned.
        /// </summary>
        /// <typeparam name="TValue">Expected type of value</typeparam>
        public static Nullable<TValue> GetValueNullable<TValue>(this ViewDataDictionary viewData, string key)
            where TValue : struct
        {
            return GetValue(viewData, key, value =>
            {
                return (TValue?)value;
            });
        }

        private static TValue GetValue<TValue>(ViewDataDictionary viewData, string key, Func<object, TValue> castOp)
        {
            object value;
            if (viewData.TryGetValue(key, out value) && value != null)
            {
                return castOp(value);
            }
            return default(TValue);
        }
    }
}