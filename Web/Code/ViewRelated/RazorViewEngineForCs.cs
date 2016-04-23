using System;
using System.Web.Mvc;

namespace RecordLabel.Web
{
    public class RazorViewEngineForCs : RazorViewEngine
    {
        public RazorViewEngineForCs()
        {
            AreaViewLocationFormats = new string[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };

            AreaMasterLocationFormats = new string[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };

            AreaPartialViewLocationFormats = new string[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };

            ViewLocationFormats = new string[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
            };

            MasterLocationFormats = new string[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };

            PartialViewLocationFormats = new string[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
        }
    }
}