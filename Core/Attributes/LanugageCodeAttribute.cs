using System;

namespace RecordLabel
{
    [AttributeUsage(AttributeTargets.Field,  AllowMultiple = false)]
    public class LanugageCodeAttribute : Attribute
    {
        public string TwoLetterISOLanguageName => code;
        private string code;

        public LanugageCodeAttribute(string twoLetterISOLanguageName)
        {
            code = twoLetterISOLanguageName;
        }
    }
}
