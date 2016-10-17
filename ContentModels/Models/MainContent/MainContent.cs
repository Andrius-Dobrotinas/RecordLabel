using System;
using System.Collections.Generic;

namespace RecordLabel.Data.Models
{
    public abstract class MainContent : Base<LocalizedString>
    {
        public virtual IList<Metadata> Metadata
        {
            get
            {
                return metadata ?? (metadata = new List<Metadata>());
            }
            set
            {
                metadata = value;
            }
        }
        private IList<Metadata> metadata { get; set; }

        [CascadeOnDelete]
        public virtual IList<Reference> References
        {
            get
            {
                return reference ?? (reference = new List<Reference>());
            }
            set
            {
                reference = value;
            }
        }
        private IList<Reference> reference { get; set; }

        [CascadeOnDelete]
        public virtual IList<Track> Tracks
        {
            get
            {
                return tracks ?? (tracks = new List<Track>());
            }
            set
            {
                tracks = value;
            }
        }
        private IList<Track> tracks { get; set; }
    }
}
