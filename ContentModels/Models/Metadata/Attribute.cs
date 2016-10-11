using System;
using System.Collections.Generic;
using System.Linq;

namespace RecordLabel.Content.Metadata
{
    public class Attribute : Metadata<Attribute>, IValueComparable<Attribute>
    {
        public AttributeType Type { get; set; }
        public virtual IList<AttributeSet> AttributeSets { get; set; }

        //TODO
        public override void Delete(ReleaseContext dbContext)
        {
            AttributeSet[] sets = dbContext.AttributeSets.Where(set => set.Collection.FirstOrDefault(item => item.Id == this.Id) != null).ToArray();
            foreach (AttributeSet set in sets)
            {
                set.Collection.Remove(this);
                //TODO: might want to delete all empty AttributeSets... However, they don't do any harm and it would be a tricky endeavour considering they can be used by pretty much any type. Would have to scan every DbSet
            }
            base.Delete(dbContext);
        }

        public bool ValuesEqual(Attribute compareTo)
        {
            return compareTo != null &&
                Type == compareTo.Type &&
                ClassHelper.CompareReferenceTypes(Localization, compareTo.Localization);
        }
    }

}
