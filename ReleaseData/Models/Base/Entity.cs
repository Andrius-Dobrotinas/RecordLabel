using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Catalogue
{
    /// <summary>
    /// A very base class for all database entity classes with no primary key
    /// </summary>
    public abstract class Entity : IHasId
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// Updates this model with to match the state of sourceModel. Implementing methods are not supposed to alter Id property.
        /// </summary>
        /// <param name="dbContext">Database context which to perform changes in</param>
        /// <param name="sourceModel">Model whose property values to copy to this model</param>
        public abstract void UpdateModel(ReleaseContext dbContext, object sourceModel);

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
