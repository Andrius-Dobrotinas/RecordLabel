using System;

namespace RecordLabel
{
    public class EntityInUseException : Exception
    {
        private string message;
        public override string Message
        {
            get
            {
                return message;
            }
        }
        public EntityInUseException(string message)
        {
            this.message = message;
        }
    }
}
