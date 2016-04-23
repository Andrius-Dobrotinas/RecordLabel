using System.Web.Mvc;

namespace RecordLabel.Web
{
    /// <summary>
    /// Used to ensure the action can be called only via Ajax
    /// </summary>
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}