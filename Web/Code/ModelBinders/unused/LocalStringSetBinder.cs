using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Web.Mvc;
using RecordLabel.Catalogue;

namespace RecordLabel.Web.ModelBinding
{
    /// <summary>
    /// Removes empty elements from the Localization model and returns null if it is empty
    /// </summary>
    public class LocalStringSetBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            LocalStringSet model = base.BindModel(controllerContext, bindingContext) as LocalStringSet;
            model.Collection.RemoveEmptyItems();
            if (model.IsEmpty)
            {
                return null;
            }
            return model;
        }
    }
}