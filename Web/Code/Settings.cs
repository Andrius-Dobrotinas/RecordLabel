using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RecordLabel.Web
{
    /// <summary>
    /// Application global settings that are initialized once on application startup
    /// </summary>
    public static class Settings
    {
        public static string CompanyName { get; set; }
        public static string LogoFileName { get; set; }

        public static int ListItemBatchSize { get; set; }
 
        public static double ThumbnailWidth { get; set; }
        public static int ThumbnailQualityLevel { get; set; }

        public static string StaticImageDirectory { get; set; }
        public static string ContentDirectory { get; set; }
        public static string ContentImageDirectory => $"{ContentDirectory}/Images";

        public static void LoadApplicationConfiguration()
        {
            RecordLabel.Localization.DefaultLanguageCode = ConfigurationManager.AppSettings.Get("DefaultLanguage");
            Content.Reference.YoutubeLinkBase = ConfigurationManager.AppSettings.Get("YoutubeLinkBase");

            ThumbnailQualityLevel = int.Parse(ConfigurationManager.AppSettings.Get("ThumbnailQualityLevel"));
            ListItemBatchSize = int.Parse(ConfigurationManager.AppSettings.Get("ListItemBatchSize"));
            CompanyName = ConfigurationManager.AppSettings.Get("CompanyName");
            LogoFileName = ConfigurationManager.AppSettings.Get("LogoFileName");
            ThumbnailWidth = double.Parse(ConfigurationManager.AppSettings.Get("ThumbnailWidth"));
            ThumbnailQualityLevel = int.Parse(ConfigurationManager.AppSettings.Get("ThumbnailQualityLevel"));

            var directories = ConfigurationManager.GetSection("directories") as System.Collections.Specialized.NameValueCollection;
            ContentDirectory = directories.Get("ContentDirectory");
            StaticImageDirectory = directories.Get("StaticImageDirectory");
        }
    }
}