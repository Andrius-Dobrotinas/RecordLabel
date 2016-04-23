using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecordLabel.Web
{
    public static class HtmlButtons
    {
        /// <summary>
        /// Builds an Html Button
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="text">Button text</param>
        /// <param name="name">Button name</param>
        /// <param name="id">Button Id</param>
        /// <param name="htmlAttributes">Extra Html attributes for the button</param>
        /// <param name="dontUseDefaultButtonClass">Tells not to use the default "btn btn-default" class</param>
        /// <param name="pullLeft">Adds a pull-left CSS class</param>
        /// <returns></returns>
        public static MvcHtmlString Button(this HtmlHelper helper, string text, string name, string id, object htmlAttributes, bool pullLeft, bool dontUseDefaultButtonClass)
        {
            TagBuilder tag = new TagBuilder("button");
            tag.MergeAttribute("type", "button");

            if (!string.IsNullOrEmpty(name))
            {
                tag.MergeAttribute("name", name);
            }

            if (!string.IsNullOrEmpty(id))
            {
                tag.MergeAttribute("id", id);
            }

            if (htmlAttributes != null)
            {
                IDictionary<string, object> attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                tag.MergeAttributes(attributes, true);
            }

            if (!dontUseDefaultButtonClass)
            {
                tag.AddCssClass("btn btn-default");
            }

            if (pullLeft)
            {
                tag.AddCssClass("pull-left");
            }

            tag.SetInnerText(text);

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string text, string name, string id, object htmlAttributes, bool pullLeft)
        {
            return helper.Button(text, name, id, htmlAttributes, pullLeft, false);
        }

        /// <summary>
        /// Builds a button for use with Modal Dialog
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="text">Button text</param>
        /// <param name="name">Button name</param>
        /// <param name="id">Button Id</param>
        /// <param name="htmlAttributes">Extra Html attributes for the button</param>
        /// <returns></returns>
        public static MvcHtmlString ButtonForModal(this HtmlHelper helper, string text, string name, string id, object htmlAttributes)
        {
            return helper.Button(text, name, id, htmlAttributes, true);
        }

        /// <summary>
        /// Builds a button for use with Modal Dialog
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="text">Button text</param>
        /// <param name="name">Button name</param>
        /// <returns></returns>
        public static MvcHtmlString ButtonForModal(this HtmlHelper helper, string text, string name)
        {
            return helper.ButtonForModal(text, name, null, null);
        }

        /// <summary>
        /// Builds a button for use with Modal Dialog
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="text">Button text</param>
        /// <param name="name">Button name</param>
        /// <param name="id">Button Id</param>
        /// <returns></returns>
        public static MvcHtmlString ButtonForModal(this HtmlHelper helper, string text, string name, string id)
        {
            return helper.ButtonForModal(text, name, id, null);
        }
    }
}