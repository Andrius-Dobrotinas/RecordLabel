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
            Catalogue.LocalStringSet.DefaultLanguage = Catalogue.LocalStringSet.ResolveLanguage(ConfigurationManager.AppSettings.Get("DefaultLanguage"));
            Catalogue.Reference.YoutubeLinkBase = ConfigurationManager.AppSettings.Get("YoutubeLinkBase");

            Settings.ThumbnailQualityLevel = int.Parse(ConfigurationManager.AppSettings.Get("ThumbnailQualityLevel"));
            Settings.ListItemBatchSize = int.Parse(ConfigurationManager.AppSettings.Get("ListItemBatchSize"));
            Settings.CompanyName = ConfigurationManager.AppSettings.Get("CompanyName");
            Settings.LogoFileName = ConfigurationManager.AppSettings.Get("LogoFileName");
            Settings.ThumbnailWidth = double.Parse(ConfigurationManager.AppSettings.Get("ThumbnailWidth"));
            Settings.ThumbnailQualityLevel = int.Parse(ConfigurationManager.AppSettings.Get("ThumbnailQualityLevel"));

            var directories = ConfigurationManager.GetSection("directories") as System.Collections.Specialized.NameValueCollection;
            Settings.ContentDirectory = directories.Get("ContentDirectory");
            Settings.StaticImageDirectory = directories.Get("StaticImageDirectory");
        }
    }
}