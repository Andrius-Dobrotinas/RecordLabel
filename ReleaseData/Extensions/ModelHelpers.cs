using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Content
{
    public static class ModelHelpers
    {
        /// <summary>
        /// Compares two class type objects that implement IValueComparable and tells whether their values are equal. Handles null cases.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <returns>True for equal values</returns>
        public static bool CompareReferenceTypes<T>(T object1, T object2) where T : IValueComparable<T>
        {
            return ((object1 == null && object2 == null) ||
                (object1?.ValuesEqual(object2) ?? false));
        }

        public static bool CompareNagivationalProperties<TModel>(TModel model1, TModel model2, Expression<Func<TModel, int?>> foreignKey)
        {
            if (foreignKey == null)
            {
                throw new ArgumentNullException("foreignKey may not be null whereas it is now");
            }
            MemberExpression mex = (MemberExpression)foreignKey.Body;
            PropertyInfo fkProperty = (PropertyInfo)mex.Member;
            ForeignKeyAttribute attr = fkProperty.GetCustomAttribute(typeof(ForeignKeyAttribute)) as ForeignKeyAttribute;
            if (attr == null)
            {
                throw new ArgumentException("foreignKey property does not containt ForeignKey attribute");
            }

            int? key1 = fkProperty.GetValue(model1) as int?;
            int? key2 = fkProperty.GetValue(model2) as int?;

            if (key1 != key2)
            {
                return false;
            }
            else if (key1 == null && key2 == null)
            {
                PropertyInfo property = typeof(TModel).GetProperty(attr.Name);
                IHasId val1 = property.GetValue(model1) as IHasId;
                IHasId val2 = property.GetValue(model2) as IHasId;

                if (val1?.Id != val2?.Id)
                {
                    return false;
                }
            }
            return true;
        }

        /*public static bool ValuesEqual<T>(T obj1, T obj2)
        {
            //Skip read-only and "Id" properties
            System.Reflection.PropertyInfo[] properties = typeof(T).GetProperties()
                .Where(prop => prop.Name != "Id" && !Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute)))
                .ToArray(); //&& prop.CanWrite == true

            //Select all properties that have ForeignKey attribute and their respective navigational properties
            KeyValuePair<System.Reflection.PropertyInfo, System.Reflection.PropertyInfo>[] foreignKeys = properties
                .Select(p => new KeyValuePair<System.Reflection.PropertyInfo, System.Reflection.PropertyInfo>
                (p, properties.SingleOrDefault(i => i.Name == p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute), true).Cast<System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute>().SingleOrDefault()?.Name)))
                .Where(keyValue => keyValue.Value != null).ToArray();


            foreach (var item in foreignKeys)
            {
                var key1 = item.Key.GetValue(obj1);
                var val1 = item.Value.GetValue(obj1);
                var key2 = item.Key.GetValue(obj2);
                var val2 = item.Value.GetValue(obj2);

            }

            foreach (System.Reflection.PropertyInfo property in properties.Where(p => foreignKeys.SingleOrDefault(fk => fk.Key == p || fk.Value == p).Equals(default(KeyValuePair<System.Reflection.PropertyInfo, System.Reflection.PropertyInfo>))).ToArray())
            {
                Type type = property.PropertyType;
                if (type.IsValueType || type.IsEnum || type == typeof(string))
                {
                    if (property.GetValue(obj1) != property.GetValue(obj2))
                    {
                        return false;
                    }
                }
                else //Class
                {
                    object val1 = property.GetValue(obj1);
                    object val2 = property.GetValue(obj2);
                    if (val1 == val2) //covers both null and reference equals situation
                    {
                        continue;
                    }
                    if (val1 != null | val2 != null)
                    {
                        return false;
                    }
                    else
                    {
                        if (type is IList<T>)
                        {
                            IList<T> list1 = (IList<T>)val1;
                            IList<T> list2 = (IList<T>)val2;
                            if (list1.Count != list2.Count)
                            {
                                return false;
                            }
                            else
                            {
                                for (int i = 0; i < list1.Count; i++)
                                {
                                    if (ValuesEqual(list1[i], list2[i]) == false)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        else //if (type.IsAssignableFrom(typeof(IValueComparable<T>)))
                        {
                            //IValueComparable<T> value1 = (IValueComparable<T>)val1;
                            //T value2 = (T)val2;
                            //value1.ValuesEqual(value2);
                            if (ValuesEqual<T>((T)val1, (T)val2) == false)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }*/
    }
}
