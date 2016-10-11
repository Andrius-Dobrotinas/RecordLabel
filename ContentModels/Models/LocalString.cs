using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace RecordLabel.Content
{
    [OneToOneRelationshipAttribute]
    public class LocalString : EntityBase, IKnowIfImEmpty, IValueComparable<LocalString>
    {
        [NotMapped]
        public override int Id { get; set; }

        [ForeignKey("LocalizationObject"), Key, Column(Order = 0)]
        public int StringSetId { get; set; }
        public LocalStringSet LocalizationObject { get; set; }

        [Key, Column(Order = 1)]
        public Language Language { get; set; }

        [AllowHtml]
        public string Text { get; set; }

        public bool IsEmpty
        {
            get
            {
                return String.IsNullOrWhiteSpace(Text);
            }
        }

        bool IValueComparable<LocalString>.ValuesEqual(LocalString compareTo)
        {
            return compareTo != null &&
                Text == compareTo.Text &&
                Language == compareTo.Language;
        }
    }
}
