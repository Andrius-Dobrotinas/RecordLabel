using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecordLabel.Web
{
    public static class HtmlExtensions
    {
        public static HtmlString RenderHtmlAttributes(this IDictionary<string, object> htmlAttributes)
        {
            return new HtmlString(String.Join(" ", htmlAttributes.Select(item => $"{item.Key}=\"{item.Value}\"")));
        }
    }
}