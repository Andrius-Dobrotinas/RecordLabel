using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;
using RecordLabel.Catalogue;
using RecordLabel.Catalogue.Attributes;

namespace RecordLabel.Catalogue
{
    [UsesGenre]
    public class Release : BaseWithImages, IHasASet<Reference>, IHasASet<Track>, IHasASet<Release>, IValueComparable<Release>
    {
        [Required]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Title")]
        public string Title { get; set; }
        
        [ForeignKey("Artist")]
        [Required]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Release_Artist")]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        [Display(ResourceType = typeof(ModelLocalization), Name = "Release_Subtitle")]
        public string Subtitle { get; set; }
        [Display(ResourceType = typeof(ModelLocalization), Name = "Release_Date")]
        [Range(1950, 2099)]
        public short? Date { get; set; }
        [Display(ResourceType = typeof(ModelLocalization), Name = "DateRecorded")]
        [Range(1950, 2099)]
        public short? DateRecorded { get; set; }
        [Display(ResourceType = typeof(ModelLocalization), Name = "Playtime")]
        public short? Playtime { get; set; }
        [Display(ResourceType = typeof(ModelLocalization), Name = "MusicBy")]
        public string MusicBy { get; set; }
        [Display(ResourceType = typeof(ModelLocalization), Name = "LyricsBy")]
        public string LyricsBy { get; set; }

        /// <summary>
        /// Indicates that other (slave) versions may be assigned to this release
        /// </summary>
        [Display(ResourceType = typeof(ModelLocalization), Name = "IsMasterVersion")]
        public bool IsMasterVersion { get; set; }

        [ForeignKey("References")]
        [Display(ResourceType = typeof(ModelLocalization), Name = "References")]
        public int? ReferencesId { get; set; }
        public ReferenceSet References { get; set; }

        [ForeignKey("Tracklist")]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Tracklist")]
        public int? TracklistId { get; set; }
        public virtual Tracklist Tracklist { get; set; }

        [ForeignKey("Descriptions")]
        public int? DescriptionsId { get; set; }
        public LocalStringSet Descriptions { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Release_Description")]
        public string Description => Descriptions?.Text;

        [Required]
        [Display(ResourceType = typeof(ModelLocalization), Name = "CatalogueNumber")]
        public string CatalogueNumber { get; set; }

        [Required]
        [ForeignKey("Media")]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Media")]
        public int MediaId { get; set; }
        public Metadata.MediaType Media { get; set; }

        [Required]
        [Display(ResourceType = typeof(ModelLocalization), Name = "PrintStatus")]
        public PrintStatus PrintStatus { get; set; } = PrintStatus.InPrint;

        /// <summary>
        /// Id of a IsMasterVersion version that this release is a slave to
        /// </summary>
        [ForeignKey("MasterVersion")]
        public int? MasterVersionId { get; set; }
        /// <summary>
        /// A list of slave releases
        /// </summary>
        public virtual Release MasterVersion { get; set; }

        /// <summary>
        /// A list of all releases (excluding this one) related through the Master version, populated by calling LoadOtherVersions
        /// </summary>    
        [NotMapped]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Release_OtherVersions")]
        public IList<Release> OtherVersions {
            get
            {
                return (otherVersions ?? (otherVersions = new List<Release>()));
            }
            set
            {
                otherVersions = value;
            }
        }
        private IList<Release> otherVersions;

        public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {
            Release source = (Release)sourceModel;
            Title = source.Title;
            Subtitle = source.Subtitle;
            Date = source.Date;
            DateRecorded = source.DateRecorded;
            Playtime = source.Playtime;
            MusicBy = source.MusicBy;
            LyricsBy = source.LyricsBy;
            CatalogueNumber = source.CatalogueNumber;
            PrintStatus = source.PrintStatus;
            IsMasterVersion = source.IsMasterVersion;
            //MasterVersionId = source.MasterVersionId;
            MasterVersion = source.MasterVersion;
            
            MediaId = source.MediaId;
            //Check if the property itself has changed
            if (source.Media != null && source.Media != Media) //non-nullable
            {
                Media = source.Media;
            }

            ArtistId = source.ArtistId;
            //Check if the property itself has changed
            if (source.Artist != null && source.Artist != Artist) //non-nullable
            {
                Artist = source.Artist;
            }

            //Descriptions
            LocalStringSet.UpdateSet<Release>(this, m => m.Descriptions, source.Descriptions, dbContext);

            //Tracklist
            Tracklist.UpdateSet(this, model => model.Tracklist, source.Tracklist, dbContext);

            //References
            ReferenceSet.UpdateSet(this, model => model.References, source.References, dbContext);

            base.UpdateModel(dbContext, sourceModel);
        }

        //TODO
        public override void Delete(ReleaseContext dbContext)
        {
            Descriptions?.Delete(dbContext);
            References?.Delete(dbContext);
            Tracklist?.Delete(dbContext);

            if (IsMasterVersion == true)
            {
                //Remove reference to this release from all releases that refer to this one as their Master version
                foreach (Release item in LoadSlaveVersions(dbContext.Releases))
                {
                    item.MasterVersionId = null;
                }
            }
            base.Delete(dbContext);
        }

        public bool ValuesEqual(Release compareTo)
        {
            return compareTo != null &&
                Title == compareTo.Title &&
                Subtitle == compareTo.Subtitle &&
                Date == compareTo.Date &&
                CatalogueNumber == compareTo.CatalogueNumber &&
                DateRecorded == compareTo.DateRecorded &&
                Playtime == compareTo.Playtime &&
                MusicBy == compareTo.MusicBy &&
                LyricsBy == compareTo.LyricsBy &&
                PrintStatus == compareTo.PrintStatus &&
                IsMasterVersion == compareTo.IsMasterVersion &&
                ModelHelpers.CompareNagivationalProperties(this, compareTo, m => m.TracklistId) &&
                ModelHelpers.CompareNagivationalProperties(this, compareTo, m => m.ReferencesId) &&
                ModelHelpers.CompareNagivationalProperties(this, compareTo, m => m.DescriptionsId) &&
                ModelHelpers.CompareNagivationalProperties(this, compareTo, m => m.MediaId) &&
                ModelHelpers.CompareNagivationalProperties(this, compareTo, m => m.ArtistId) &&
                ModelHelpers.CompareReferenceTypes(Localization, compareTo.Localization);
        }

        /// <summary>
        /// Loads all other versions of this release.
        /// </summary>
        /// <param name="releaseDbSet"></param>
        public void LoadOtherVersions(DbSet<Release> releaseDbSet)
        {
            LoadOtherVersions(releaseDbSet, null);
        }

        /// <summary>
        /// Loads all other versions of this release.
        /// </summary>
        /// <param name="releaseDbSet"></param>
        public void LoadOtherVersions(DbSet<Release> releaseDbSet, Func<IQueryable<Release>, IQueryable<Release>> extraOperationsOnSelectedRelease)
        {
            if (IsMasterVersion == true)
            {
                OtherVersions = LoadSlaveVersions(releaseDbSet);
            }
            else if (MasterVersion != null)
            {
                OtherVersions = releaseDbSet.Where(item => item.MasterVersionId == this.MasterVersionId && item.Id != this.Id).ToList();
                OtherVersions.Add(MasterVersion);
            }
        }

        private IList<Release> LoadSlaveVersions(DbSet<Release> releaseDbSet)
        {
            return releaseDbSet.Where(item => item.MasterVersionId == this.Id).ToList();
        }
    }
}
