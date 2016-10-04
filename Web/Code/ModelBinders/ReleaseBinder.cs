using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using RecordLabel.Content;
using RecordLabel.Web.Controllers;

using System.Linq.Expressions;
using System.Reflection;

namespace RecordLabel.Web.ModelBinding
{
    /// <summary>
    /// Selects all Slave Versions from the database and adds them to the model
    /// </summary>
    public class ReleaseBinder : BaseWithImagesBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Release model = (Release)base.BindModel(controllerContext, bindingContext);

            // Remove MasterVersionId if it's 0 because that means it doesn't refer to any master release
            if (model.MasterVersionId.HasValue && model.MasterVersionId.Value == 0)
            {
                model.MasterVersionId = null;
            }
            return model;
        }
    }
}