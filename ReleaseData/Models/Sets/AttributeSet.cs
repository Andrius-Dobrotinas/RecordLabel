using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordLabel.Catalogue.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace RecordLabel.Catalogue
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