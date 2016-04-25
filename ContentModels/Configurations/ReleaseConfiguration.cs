using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace RecordLabel.Content.Configurations
{
    public class ReleaseConfiguration : EntityTypeConfiguration<Release>
    {
        /*public ReleaseConfiguration()
        {
            //this.HasRequired(release => release.Items);
            this.HasMany(release => release.Items).WithRequired().WillCascadeOnDelete();
        }*/
    }
}
