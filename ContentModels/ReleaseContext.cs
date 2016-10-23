using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using RecordLabel.Data.Models;

namespace RecordLabel.Data.Context
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ReleaseContext : DbContext
    {
        public DbSet<MainContent> ContentEntries { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
        public DbSet<Metadata> Metadata { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackReference> TrackReferences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MainContent>()
                .HasMany(x => x.Metadata)
                .WithMany(x => x.Targets)
                .Map(x => x.ToTable("Metadata", "JoinTables"));

            modelBuilder.Entity<MainContent>()
                .HasMany(x => x.LocalizedText)
                .WithRequired(x => x.TargetObject)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Metadata>()
                .HasMany(x => x.LocalizedText)
                .WithRequired(x => x.TargetObject)
                .WillCascadeOnDelete();

            modelBuilder.Entity<MediaType>()
                .HasMany(x => x.LocalizedText)
                .WithRequired(x => x.TargetObject)
                .WillCascadeOnDelete();

            modelBuilder.Entity<MainContent>()
                .HasMany(x => x.References)
                .WithRequired(x => x.Owner);

            modelBuilder.Entity<Reference>()
                .ToTable("MainContent", "References");

            modelBuilder.Entity<MainContent>()
                .HasMany(x => x.Tracks)
                .WithRequired(x => x.MainContent);

            modelBuilder.Entity<Track>()
                .HasOptional(x => x.Reference)
                .WithRequired(x => x.Track)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TrackReference>()
                .HasRequired(x => x.Track)
                .WithOptional(x => x.Reference)
                .WillCascadeOnDelete(); // For some reason this doesn't work in this case

            modelBuilder.Entity<Release>().ToTable("Releases");
            modelBuilder.Entity<Artist>().ToTable("Artists");
            modelBuilder.Entity<Article>().ToTable("Articles");
            modelBuilder.Entity<Metadata>().ToTable("Metadata");
            //modelBuilder.Entity<MediaType>().ToTable("MediaTypes");

            modelBuilder.Entity<LocalizedString>().ToTable("MainContent", "LocalizedStrings");
            modelBuilder.Entity<MediaTypeLocalizedString>().ToTable("MediaTypes", "LocalizedStrings");
            modelBuilder.Entity<MetadataLocalizedString>().ToTable("Metadata", "LocalizedStrings");
        }
    }
}
