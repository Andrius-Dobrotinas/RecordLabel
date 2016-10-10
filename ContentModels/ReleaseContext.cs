using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecordLabel.Content
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ReleaseContext : DbContext
    {
        public DbSet<Release> Releases { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<AttributeSet> AttributeSets { get; set; }
        public DbSet<Metadata.MediaType> MediaTypes { get; set; }
        public DbSet<Metadata.Attribute> Attributes { get; set; }
        public DbSet<ImageSet> ImageSets { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<LocalStringSet> LocalStringSet { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<ReferenceSet> ReferenceSets { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Tracklist> Tracklists { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Configurations.Add(new Configurations.ReleaseConfiguration());
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<AttributeSet>().HasMany(x => x.Collection).WithMany(x => x.AttributeSets).Map(x => x.ToTable("AttributeMap", "JoinTables"));
            modelBuilder.Entity<LocalStringSet>().HasMany(x => x.Collection).WithRequired(y => y.LocalizationObject).WillCascadeOnDelete();

            modelBuilder.Entity<Release>().HasOptional<LocalStringSet>(x => x.Descriptions).WithOptionalPrincipal();
            modelBuilder.Entity<Release>().HasOptional<LocalStringSet>(x => x.Localization).WithOptionalPrincipal();
            modelBuilder.Entity<Artist>().HasOptional<LocalStringSet>(x => x.Localization).WithOptionalPrincipal();
        }
    }
}
