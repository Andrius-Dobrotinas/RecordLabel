using System;
using System.Collections.Generic;

namespace RecordLabel.Data.Models
{
    public class Metadata : Base<MetadataLocalizedString>
    {
        public MetadataType Type { get; set; }

        public virtual IList<MainContent> Targets
        {
            get
            {
                return targets ?? (targets = new List<MainContent>());
            }
            set
            {
                targets = value;
            }
        }
        private IList<MainContent> targets { get; set; }
    }
}