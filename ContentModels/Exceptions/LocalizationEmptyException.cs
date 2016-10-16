using System;

namespace RecordLabel.Data.Models
{
    public class LocalizationEmptyException : Exception
    {
        public LocalizationEmptyException() : base("Localization property of this model is not allowed to be empty")
        {

        }
    }
}
