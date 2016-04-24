using System;
using System.Web.Mvc;

namespace RecordLabel.Web
{
    public class DefaultExceptionHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //log

            HandledException handledException = filterContext.Exception as HandledException;
            //If it's not a Handled exception, and we're not in admin mode, just show a generic error message
            if (handledException == null && !Global.IsAdminMode)
            {
                handledException = new HandledException(Localization.ApplicationLocalization.UnexpectedError);
            }

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = View,
                ViewData = new ViewDataDictionary(handledException ?? filterContext.Exception)
            };
        }
    }
}