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
    /// Removes empty elements from the Tracklist model and returns null if it is empty
    /// </summary>
    public class TracklistBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Tracklist model = base.BindModel(controllerContext, bindingContext) as Tracklist;
            model.Collection.RemoveEmptyItems();
            if (model.IsEmpty)
            {
                return null;
            }
            return model;
        }
    }
}