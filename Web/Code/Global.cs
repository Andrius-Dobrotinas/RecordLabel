using RecordLabel.Content;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace RecordLabel.Web
{
    public static class Global
    {
        public static bool IsAdminMode
        {
            get
            {
                return (HttpContext.Current.Session?["AdminMode"] as bool?) == true;
            }
        }

        public static ViewMode CurrentMode
        {
            get
            {
                return IsAdminMode ? ViewMode.Admin : ViewMode.User;
            }
        }

        public static string CurrentLanguage
        {
            get
            {
                return RecordLabel.Localization.ResolveLanguage(System.Threading.Thread.CurrentThread.CurrentCulture).ToString();
            }
        }

        public static void SetCurrentLanguage(string lang)
        {
            if (String.IsNullOrEmpty(lang))
            {
                lang = RecordLabel.Localization.DefaultLanguageCode;
            }
            HttpContext.Current.Session["Culture"] = CultureInfo.GetCultureInfo(lang);
        }
    }
}