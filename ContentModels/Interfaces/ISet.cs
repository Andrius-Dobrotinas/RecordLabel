using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Content
{
    public interface ISet<T> : IKnowIfImEmpty
        where T : EntityBase, IValueComparable<T>
    {
        IList<T> Collection { get; }
    }
}
