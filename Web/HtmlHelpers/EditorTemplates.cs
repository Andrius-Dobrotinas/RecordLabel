using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using RecordLabel.Catalogue;

namespace RecordLabel.Web
{
    public static class EditorTemplates
    {
        /// <summary>
        /// Builds an editor for a set of a certain type which provides a way to add/remove items. Requires EditorTemplateFor to function.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="setProperty">Model property which to build the editor for</param>
        /// <param name="containerHtmlElementId">An Id for a resulting Html element</param>
        /// <returns></returns>
        public static MvcHtmlString EditorForASet<TModel, TProperty, T>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> setProperty, string containerHtmlElementId) where TModel : IHasASet<T> where TProperty : Set<T> where T : Entity, IValueComparable<T>//constraint here is just to make sure that we operate on types that are accepted by the Set class
        {
            if (String.IsNullOrWhiteSpace(containerHtmlElementId))
            {
                throw new ArgumentException();
            }

            MemberExpression mex = (MemberExpression)setProperty.Body;
            PropertyInfo property = mex.Member as PropertyInfo;
            PropertyInfo collectionProperty = property.PropertyType.GetProperties().Single(prop => prop.PropertyType == typeof(IList<T>));

            //Create a container div
            TagBuilder tag = new TagBuilder("div");
            tag.MergeAttribute("id", containerHtmlElementId);
            tag.MergeAttribute("data-container-for", typeof(T).Name); //element name that this is a container for

            if (html.ViewData.Model != null)
            {
                Set<T> model = property.GetValue(html.ViewData.Model) as Set<T>;
            
                if (model != null)
                {
                    //Build property prefix
                    string prefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
                
                    //Preserve original prefix if present
                    if (string.IsNullOrEmpty(prefix) == false)
                    {
                        prefix = prefix + ".";
                    }
                    prefix = $"{prefix}{property.Name}.{collectionProperty.Name}";

                    //Build html attributes for each item
                    IDictionary<string, object> attribues = new Dictionary<string, object>(); //HtmlHelper.AnonymousObjectToHtmlAttributes(null);
                    attribues.Add("data-from-model", null); //tells that this item is in the database

                    var viewData = new ViewDataDictionary();

                    viewData.Add("htmlAttributes", attribues);
                    viewData.Add("indexFieldName", $"{prefix}.Index");
                    viewData.Add("typeName", typeof(T).Name);
                    viewData.Add("isTemplate", false);

                    viewData.Add("index", null);
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < model.Collection.Count; i++)
                    {
                        //Html.EditorFor(model => Model.Item1.Collection[i])
                        viewData["index"] = i;
                        viewData.TemplateInfo.HtmlFieldPrefix = $"{prefix}[{i}]";
                        MvcHtmlString view = html.Partial("ExtensionPartials/Template", model.Collection[i], viewData);
                        stringBuilder.AppendLine(view.ToString());
                    }
                    tag.SetInnerText(stringBuilder.ToString());
                }
            }
            return new MvcHtmlString(HttpUtility.HtmlDecode(tag.ToString()));
        }

        /// <summary>
        /// Builds an editor template for a collection property of a model used for adding new items while editing 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="property">Collection property which to build the editor for</param>
        /// <returns></returns>
        public static HtmlString EditorTemplateFor<TModel, T>(this HtmlHelper<TModel> html, Expression<Func<TModel, IList<T>>> property) where T : Entity
        {
            IList<PropertyInfo> propertyTree = Reflection.Reflection.GetPropertyTree((MemberExpression)property.Body);

            string propertyName = propertyTree.JoinValues<PropertyInfo>(item => item.Name, '.');
            string prefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            //Preserve original prefix if present
            if (string.IsNullOrEmpty(prefix) == false)
            {
                prefix = prefix + ".";
            }
            prefix = $"{prefix}{propertyName}";

            var viewData = new ViewDataDictionary();

            IDictionary<string, object> attribues = new Dictionary<string, object>(); //HtmlHelper.AnonymousObjectToHtmlAttributes(null);
            attribues.Add("data-property-name", prefix);
            attribues.Add("data-property-id", prefix.Replace('.', '_'));

            viewData.Add("htmlAttributes", attribues);
            viewData.Add("indexFieldName", $"{prefix}.Index");
            viewData.Add("typeName", typeof(T).Name);
            viewData.Add("isTemplate", true);

            return html.Partial("ExtensionPartials/Template", viewData);
        }
    }
}