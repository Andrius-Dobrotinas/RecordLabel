using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Catalogue
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OneToOneRelationship : Attribute
    {

    }
}
