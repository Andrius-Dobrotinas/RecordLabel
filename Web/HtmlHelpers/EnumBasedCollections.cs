using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;
using RecordLabel.Reflection;

namespace RecordLabel.Web
{
    public static class EnumBasedCollections
    {
        /// <summary>
        /// A model that defines CSS classes for elements rendered by EditorForEnumBasedCollection
        /// </summary>
        public class CssClasses
        {
            public CssClasses(string rootContainerClass, string inputLabelClass, string inputContainerClass, string inputClass)
            {
                RootContainerClass = rootContainerClass;
                InputLabelClass = inputLabelClass;
                InputContainerClass = inputContainerClass;
                InputClass = inputClass;
            }
            public string RootContainerClass { get; set; }
            public string InputLabelClass { get; set; }
            public string InputContainerClass { get; set; }
            public string InputClass { get; set; }
        }

        /// <summary>
        /// Type of input fields built by EditorForEnumBasedCollection
        /// </summary>
        public enum InputFieldType
        {
            SingleLine = 0,
            TextArea = 1,
            TextAreaModal = 2,
        }

        public static MvcHtmlString EditorForEnumBasedCollection<TModel, TCollectionItem, TEnumProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IList<TCollectionItem>>> collectionPropertyExpression, Expression<Func<TCollectionItem, TEnumProperty>> enumPropertyExpression, Expression<Func<TCollectionItem, object>> valuePropertyExpression, CssClasses cssClasses) where TModel : Catalogue.Base where TEnumProperty : struct, IConvertible, IComparable, IFormattable
        {
            return EditorForEnumBasedCollection(helper, collectionPropertyExpression, enumPropertyExpression, valuePropertyExpression, cssClasses, InputFieldType.SingleLine);
        }

        /// <summary>
        /// Builds a text input element for each element in the enum-based model property using its enum property as a key
        /// </summary>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <typeparam name="TCollectionItem">Type of the property of the model which to render the inputs for</typeparam>
        /// <typeparam name="TEnumProperty">Type of enum property that's used as a key</typeparam>
        /// <param name="helper"></param>
        /// <param name="collectionPropertyExpression">Collection property of the model which to render the inputs for</param>
        /// <param name="enumPropertyExpression">Property of Enum type that's used as an identifying key for the object and is hidden in the form</param>
        /// <param name="valuePropertyExpression">Property to build the inputs for</param>
        /// <param name="cssClasses">Object that contains css class names for elements built by this helper</param>
        /// <param name="inputFieldType">Type of value input fields</param>
        /// <returns></returns>
        public static MvcHtmlString EditorForEnumBasedCollection<TModel, TCollectionItem, TEnumProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IList<TCollectionItem>>> collectionPropertyExpression, Expression<Func<TCollectionItem, TEnumProperty>> enumPropertyExpression, Expression<Func<TCollectionItem, object>> valuePropertyExpression, CssClasses cssClasses, InputFieldType inputFieldType) where TModel : Catalogue.Base where TEnumProperty : struct, IConvertible, IComparable, IFormattable
            //This TEnumProperty constraint is supposed to ensure that only Enum types are passed
        {
            TModel model = helper.ViewData.Model;

            MemberInfo enumProperty = Reflection.Reflection.GetMemberInfo(enumPropertyExpression);
            MemberInfo valueProperty = Reflection.Reflection.GetMemberInfo(valuePropertyExpression);
            IList<MemberInfo> collectionPropertyTree = Reflection.Reflection.GetMemberInfoRecursive(collectionPropertyExpression); //Ordered list that contains all properties that lead from TModel type to the target property
            string propertyQualifiedName = collectionPropertyTree.JoinValues(item => item.Name, '.');// String.Join(".", collectionPropertyTree);// Reflection.GenerateFullPropertyName(collectionPropertyTree);
            string keyPropertyName = enumProperty.Name;
            string valuePropertyName = valueProperty.Name;

            IList<object> enumValues = GetEnumPropertyValues(enumProperty); //All values of the supplied Enum type
            IDictionary<object, object> modelPropertyValues = new Dictionary<object, object>(); //Values that the model's TCollectionItem collection property contains

            //Get all values from model's TCollectionItem collection property
            if (model != null)
            {
                IList<TCollectionItem> modelValues = GetCollectionPropertyValues<TModel, TCollectionItem>(model, collectionPropertyTree);                
                foreach (TCollectionItem item in modelValues)
                {
                    object key = Reflection.Reflection.GetPropertyValue<TCollectionItem>(item, enumProperty);
                    object value = Reflection.Reflection.GetPropertyValue<TCollectionItem>(item, valueProperty);
                    modelPropertyValues.Add(key, value);
                }
            }

            StringBuilder output = new StringBuilder();

            foreach (var item in enumValues)
            {
                string value = null;

                //Get model's value for this enumeration value
                if (model != null)
                {
                    if (modelPropertyValues.ContainsKey(item))
                    {
                        value = (string)modelPropertyValues[item];
                    }
                }

                int id = (int)item;
                output.AppendLine($"<div{GetHtmlClassString(cssClasses?.RootContainerClass)}>");

                MvcHtmlString htmlLabel = helper.Label($"{propertyQualifiedName}[{id}].{valuePropertyName}", item.ToString(), new { @class = cssClasses?.InputLabelClass });

                output.AppendLine(htmlLabel.ToHtmlString());

                output.AppendLine($"<div{GetHtmlClassString(cssClasses?.InputContainerClass)}>");

                MvcHtmlString htmlKey = helper.Hidden($"{propertyQualifiedName}[{id}].{keyPropertyName}", id);

                string valueInputHtml;
                string valueInputName = $"{propertyQualifiedName}[{id}].{valuePropertyName}";
                object valueInputHtmlAttributes = new { @class = cssClasses?.InputClass, check_for_val = "true" };
                //check_for_val is an attribute that tells to check this input element's value when checking if model is empty on the client side using Javascript

                if (inputFieldType == InputFieldType.TextArea || inputFieldType == InputFieldType.TextAreaModal)
                {
                    if (inputFieldType == InputFieldType.TextAreaModal)
                    {
                        MvcHtmlString input = helper.TextArea(valueInputName, value, new { @class = cssClasses?.InputClass });
                        string modalId = EscapeDots($"modal_{propertyQualifiedName}{id}");

                        MvcHtmlString ModalOpenButton = helper.ModalOpenButton(modalId, "Edit");
                        MvcHtmlString modal = helper.ModalDialog(modalId, input, item.ToString());
                        valueInputHtml = ModalOpenButton.ToString() + modal.ToString();
                    }
                    else
                    {
                        valueInputHtml = helper.TextArea(valueInputName, value, valueInputHtmlAttributes).ToHtmlString();
                    }
                }
                else
                {
                    //helper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix contains model name info
                    valueInputHtml = helper.EditorFor(m => value, null, valueInputName, new { htmlAttributes = valueInputHtmlAttributes }).ToString();
                }

                output.AppendLine(htmlKey.ToHtmlString());
                output.AppendLine(valueInputHtml);

                output.AppendLine("</div></div>");
            }
            return new MvcHtmlString(output.ToString());
        }

        private static string EscapeDots(string text)
        {
            return text.Replace(".", String.Empty);
        }

        private static string GetHtmlClassString(string cssClass) {
            return String.IsNullOrWhiteSpace(cssClass) ? String.Empty : $" class=\"{cssClass.Trim()}\"";
        }

        /// <summary>
        /// Gets all values of a given Enum-typed property
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <exception cref="ArgumentException">TEnum is not an Enum type</exception>
        /// <returns></returns>
        private static IList<object> GetEnumPropertyValues(MemberInfo memberInfo)
        {
            PropertyInfo propertyInfo = (PropertyInfo)memberInfo;

            if (propertyInfo.PropertyType.IsEnum == false)
            {
                throw new ArgumentException("TEnum must be an Enum type");
            }
            return CastEnumArrayToObjects(propertyInfo.PropertyType.GetEnumValues());
        }

        /// <summary>
        /// Converts an array of enum type values to a list of objects
        /// </summary>
        /// <param name="enumValues"></param>
        /// <returns></returns>
        private static IList<object> CastEnumArrayToObjects(Array enumValues)
        {
            List<object> result = new List<object>();
            foreach (object item in enumValues)
            {
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Gets values of TModel's property whose full path is defined by the list of properties 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TCollectionItem">Type of values the property contains</typeparam>
        /// <param name="model">Source model that the property belongs to</param>
        /// <param name="mimfo">List of properties representing property tree starting with the first property of TModel and ending with the actual property to get values for</param>
        /// <returns></returns>
        public static IList<TCollectionItem> GetCollectionPropertyValues<TModel, TCollectionItem>(TModel model, IList<MemberInfo> mimfo)
        {
            object value = model;
            for (int i = 0; i < mimfo.Count; i++)
            {
                value = ((PropertyInfo)mimfo[i]).GetValue(value);
                if (value == null)
                {
                    return new List<TCollectionItem>(0);
                }
            }
            return value as List<TCollectionItem>;
        }
    }
}