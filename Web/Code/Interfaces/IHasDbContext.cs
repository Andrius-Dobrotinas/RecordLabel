using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordLabel.Catalogue;

namespace RecordLabel.Web
{
    /// <summary>
    /// Indicates that a model has a reference to a database context
    /// </summary>
    public interface IHasDbContext
    {
        ReleaseContext DbContext { get; }
    }
}
