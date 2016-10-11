using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordLabel.Content
{
    /// <summary>
    /// A base class for all database entities that contain a localized text property and attributes
    /// </summary>
    public abstract class BaseWithAttributes : Base, IHasASet<Metadata.Attribute>
    {
        public virtual AttributeSet Attributes { get; set; }

        public override void Delete(ReleaseContext dbContext)
        {
            Attributes?.Delete(dbContext);
            base.Delete(dbContext);
        }
    }
}
