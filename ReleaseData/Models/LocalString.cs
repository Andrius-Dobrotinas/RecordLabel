using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using schema = System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using RecordLabel.Content;

namespace RecordLabel.Content
{
    public class LocalString : Entity, IKnowIfImEmpty, IValueComparable<LocalString>
    {
        [schema.ForeignKey("LocalizationObject"), Key, schema.Column(Order = 0)]
        public int StringSetId { get; set; }
        public LocalStringSet LocalizationObject { get; set; }

        [Key, schema.Column(Order = 1)]
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

        public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {
            Text = (sourceModel as LocalString)?.Text;
        }

        bool IValueComparable<LocalString>.ValuesEqual(LocalString compareTo)
        {
            return compareTo != null &&
                Text == compareTo.Text &&
                Language == compareTo.Language;
        }
    }
}
