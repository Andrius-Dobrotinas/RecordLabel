using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Catalogue
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
