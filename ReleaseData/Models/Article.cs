using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Catalogue
{
    public class Article : BaseWithImages, IHasASet<Reference>
    {
        [Display(ResourceType = typeof(ModelLocalization), Name = "Article_Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Display(ResourceType = typeof(ModelLocalization), Name = "Article_Type")]
        public ArticleType Type { get; set; }

        [Display(ResourceType = typeof(ModelLocalization), Name = "Article_Author")]
        public string Author { get; set; }

        [ForeignKey("Titles")]
        public int? TitlesId { get; set; }
        public LocalStringSet Titles { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Article_Title")]
        public string Title => Titles?.Text;

        [ForeignKey("References")]
        public int? ReferencesId { get; set; }
        [Display(ResourceType = typeof(ModelLocalization), Name = "References")]
        public ReferenceSet References { get; set; }

        public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {
            Article source = (Article)sourceModel;
            LocalStringSet.UpdateSet<Article>(this, m => m.Titles, source.Titles, dbContext);
            ReferenceSet.UpdateSet(this, model => model.References, source.References, dbContext);
            base.UpdateModel(dbContext, sourceModel);            
        }

        public override void Delete(ReleaseContext dbContext)
        {
            Titles?.Delete(dbContext);
            References?.Delete(dbContext);
            base.Delete(dbContext);
        }
    }
}
