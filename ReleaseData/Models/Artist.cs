using RecordLabel.Catalogue;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecordLabel.Catalogue.Attributes;

namespace RecordLabel.Catalogue
{
    [UsesGenre]
    public class Artist : BaseWithImages, IHasASet<Reference>
    {
        [Required]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Artist_Name")]
        public string Name { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Artist_Description")]
        public string Description => base.Text;

        
        [Display(ResourceType = typeof(ModelLocalization), Name = "Releases")]
        public virtual IList<Release> Releases
        {
            get
            {
                return releases ?? (releases = new List<Release>());
            }
            set {
                releases = value;
            }
        }
        private IList<Release> releases;

        [ForeignKey("References")]
        public int? ReferencesId { get; set; }
        [Display(ResourceType = typeof(ModelLocalization), Name = "References")]
        public ReferenceSet References { get; set; }

        public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {
            base.UpdateModel(dbContext, sourceModel);

            Artist model = (Artist)sourceModel;
            Name = model.Name;
            Releases.UpdateCollection(model.Releases);
            References.UpdateModel(dbContext, model.References);
        }

        public override void Delete(ReleaseContext dbContext)
        {
            if (Releases.Count > 0)
            {
                throw new EntityInUseException("Certain releases are associated with this artist");
            }
            References?.Delete(dbContext);
            base.Delete(dbContext);
        }
    }
}