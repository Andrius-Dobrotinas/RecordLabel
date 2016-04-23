using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace RecordLabel.Web
{
    public static class SelectListExtensions
    {
        public static SelectListItem[] ToListOfSelectListItems<T>(this IList<T> collection, Func<T, string> value, Func<T, string> text) where T : class
        {
            return collection.ToListOfSelectListItems(value, text, null, null);
        }

        public static SelectListItem[] ToListOfSelectListItems<T>(this IList<T> collection, Func<T, string> value, Func<T, string> text, Func<T, bool> selected) where T : class
        {
            return collection.ToListOfSelectListItems(value, text, selected, null);
        }

        public static SelectListItem[] ToListOfSelectListItems<T>(this IList<T> collection, Func<T, string> value, Func<T, string> text, Func<T, SelectListGroup> group) where T : class
        {
            return collection.ToListOfSelectListItems(value, text, null, group);
        }

        public static SelectListItem[] ToListOfSelectListItems<T>(this IList<T> collection, Func<T, string> value, Func<T, string> text, Func<T, bool> selected, Func<T, SelectListGroup> group) where T : class
        {
            SelectListItem[] list = new SelectListItem[collection.Count];
            for (int i = 0; i < collection.Count; i++)
            {
                list[i] = new SelectListItem() { Value = value(collection[i]), Text = text(collection[i]), Selected = (selected != null ? selected(collection[i]) : false), Group = (group != null ? group(collection[i]) : null) };
            }
            return list;
        }

        public static SelectListItem[] NewListWithFirstDefaultItem(this IList<SelectListItem> collection, string value, string text)
        {
            List<SelectListItem> newList = new List<SelectListItem>(collection.Count + 1);
            newList.Add(new SelectListItem() { Value = value, Text = text });
            newList.AddRange(collection);
            return newList.ToArray();
        }
    }
}
 