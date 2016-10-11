using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Content
{
    /// <summary>
    /// A very base class for all database entity classes with no primary key
    /// </summary>
    public abstract class EntityBase : IHasId
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// Implementing methods must delete all related entities from database context that cannot exist without this model
        /// </summary>
        /// <param name="dbContext">Database context that this model is attached to</param>
        public virtual void Delete(ReleaseContext dbContext)
        {
            dbContext.Entry(this).State = EntityState.Deleted;
        }
    }
}
