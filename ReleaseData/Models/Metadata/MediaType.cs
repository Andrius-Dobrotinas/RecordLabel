using System;
using System.Collections.Generic;
using System.Linq;
using RecordLabel.Content;

namespace RecordLabel.Content.Metadata
{
    public class MediaType : Metadata<MediaType>
    {
        public override void Delete(ReleaseContext dbContext)
        {
            if (dbContext.Releases.Any(item => item.MediaId == this.Id))
            {
                throw new EntityInUseException("Certain releases are using this media type");
            }
            base.Delete(dbContext);
        }
    }
}
