using System;

namespace RecordLabel.Web
{
    /// <summary>
    /// Exception that has been handled and is to be displayed to the user
    /// </summary>
    public class HandledException : Exception
    {
        public HandledException(string message) : base(message)
        {

        }
    }
}