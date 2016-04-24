using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordLabel.Content.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace RecordLabel.Content
{
    public class AttributeSet : Set<Metadata.Attribute>
    {
        public AttributeSet()
        {
        }

        public AttributeSet(IList<Metadata.Attribute> attributeList)
        {
            Collection = attributeList;
        }
    }
}