using System;

namespace RecordLabel.Data.Models
{
    /// <summary>
    /// A very base class for all database entity classes with no primary key
    /// </summary>
    public abstract class EntityBase : IHasId
    {
        public virtual int Id { get; set; }
    }
}
