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
    /// Removes empty elements from the ReferenceSet model and returns null if it is empty
    /// </summary>
    public class ReferenceSetBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ReferenceSet model = base.BindModel(controllerContext, bindingContext) as ReferenceSet;
            model.Collection.RemoveEmptyItems();
            if (model.IsEmpty)
            {
                return null;
            }
            return model;
        }
    }
}