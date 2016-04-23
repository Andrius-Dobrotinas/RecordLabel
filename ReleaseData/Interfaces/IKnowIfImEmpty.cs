using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Catalogue
{
    public interface IKnowIfImEmpty
    {
        bool IsEmpty { get; }
    }
}
