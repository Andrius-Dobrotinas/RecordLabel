using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecordLabel.Web
{
    public class AttributeBuilder
    {
        static string equals = "=";
        static string space = " ";
        StringBuilder builder = new StringBuilder();

        public void AddAttribute(string name)
        {
            builder.Append(name);
            builder.Append(space);
        }

        public void AddAttribute(string name, object value)
        {
            builder.Append(name);
            builder.Append(equals);
            builder.Append(value);
            builder.Append(space);
        }

        public HtmlString RenderAttributes()
        {
            return new HtmlString(builder.ToString());
        }
    }
}