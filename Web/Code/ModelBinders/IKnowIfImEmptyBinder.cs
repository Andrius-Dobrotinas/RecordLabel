using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecordLabel.Content;

namespace RecordLabel.Web.ModelBinding
{
    /// <summary>
    /// Makes sure empty models that implement IKnowIfImEmpty interface are returned as null
    /// </summary>
    public class IKnowIfImEmptyBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IKnowIfImEmpty model = (IKnowIfImEmpty)base.BindModel(controllerContext, bindingContext);
            if (model.IsEmpty)
            {
                return null;
            }
            return model;
        }
    }
}