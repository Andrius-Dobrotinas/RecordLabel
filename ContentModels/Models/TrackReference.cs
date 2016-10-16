using System;

namespace RecordLabel.Data.Models
{
    public class TrackReference : ReferenceBase
    {
        //[Key, ForeignKey("Track")]
        public override int Id { get; set; }
        public virtual Track Track { get; set; }
    }
}
