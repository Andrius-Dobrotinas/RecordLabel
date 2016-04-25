using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecordLabel.Content;

namespace RecordLabel.Web.ModelBinding
{
    /// <summary>
    /// Removes empty elements from the Set and returns null if it is empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SetBinder<T> : DefaultModelBinder where T : EntityBase, IKnowIfImEmpty, IValueComparable<T>
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object obj = base.BindModel(controllerContext, bindingContext);
            Set<T> model = obj as Set<T>;
            if (model != null)
            {
                model.Collection.RemoveEmptyItems();
                if (model.IsEmpty)
                {
                    return null;
                }
            }
            return model;
        }
    }
}