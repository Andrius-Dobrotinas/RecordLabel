using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using RecordLabel.Content;
using RecordLabel.Web.Controllers;

namespace RecordLabel.Web.ModelBinding
{
    /// <summary>
    /// Selects attributes that correspond to posted AttributeIds from the database and adds them to the Attributes collection
    /// </summary>
    public class AttributeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object model = base.BindModel(controllerContext, bindingContext);

            ReleaseContext dbContext = ((IHasDbContext)controllerContext.Controller).DbContext;
            NameValueCollection form = controllerContext.HttpContext.Request.Form;
            if (form.AllKeys.Contains("AttributeIds"))
            {
                int[] ids = form.GetValues("AttributeIds").Select(item => int.Parse(item)).ToArray();
                ((BaseWithAttributes)model).Attributes = new AttributeSet(dbContext.Attributes.Where(item => ids.Contains(item.Id)).ToArray());
            }

            return model;
        }
    }
}