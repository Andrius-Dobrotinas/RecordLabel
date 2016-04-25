using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RecordLabel
{
    public static class Localization
    {
        public static Language CurrentLanguage => ResolveLanguage(CultureInfo.CurrentCulture);//Thread.CurrentThread.CurrentCulture

        /// <summary>
        /// Default language's Two Letter ISO Language Name
        /// </summary>
        public static string DefaultLanguageCode
        {
            get
            {
                return defaultLanguageCode;
            }
            set
            {
                defaultLanguageCode = value;
                defaultLanguage = ResolveLanguage(defaultLanguageCode);
            }
        }
        public static Language DefaultLanguage
        {
            get
            {
                return defaultLanguage;
            }
        }
        private static string defaultLanguageCode;
        private static Language defaultLanguage;

        /// <summary>
        /// Cache for language code => language enum value
        /// </summary>
        private static readonly Dictionary<string, Language> languageCache;

        static Localization()
        {
            Type type = typeof(Language);
            Array values = type.GetEnumValues();
            languageCache = new Dictionary<string, Language>(values.Length);
            foreach (Language item in values)
            {
                System.Reflection.MemberInfo mimfo = type.GetMember(item.ToString())[0];
                LanugageCodeAttribute langCode = (LanugageCodeAttribute)mimfo.GetCustomAttributes(typeof(LanugageCodeAttribute), false)[0];
                languageCache.Add(langCode.TwoLetterISOLanguageName, item);
            }
        }

        /// <summary>
        /// Gets Language enum value that corresponds to the language of the supplied culture info
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static Language ResolveLanguage(CultureInfo cultureInfo)
        {
            return ResolveLanguage(cultureInfo.TwoLetterISOLanguageName);
        }

        /// <summary>
        /// Gets Language enum value that corresponds to the supplied language code
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static Language ResolveLanguage(string twoLetterISOLanguageName)
        {
            if (languageCache.ContainsKey(twoLetterISOLanguageName))
            {
                return languageCache[twoLetterISOLanguageName];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns two-letter language code associated with the supplied Language enumeration value
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetLanguageCode(Language language)
        {
            try {
                return languageCache.First(item => item.Value == language).Key;
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
