using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecordLabel.Content.Localization;

namespace RecordLabel.Content
{
    public class Article : BaseWithImages, IHasASet<Reference>
    {
        [Display(ResourceType = typeof(ContentLocalization), Name = "Article_Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Display(ResourceType = typeof(ContentLocalization), Name = "Article_Type")]
        public ArticleType Type { get; set; }

        [Display(ResourceType = typeof(ContentLocalization), Name = "Article_Author")]
        public string Author { get; set; }

        [ForeignKey("Titles")]
        public int? TitlesId { get; set; }
        public LocalStringSet Titles { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(ContentLocalization), Name = "Article_Title")]
        public string Title => Titles?.Text;

        [ForeignKey("References")]
        public int? ReferencesId { get; set; }
        [Display(ResourceType = typeof(ContentLocalization), Name = "References")]
        public ReferenceSet References { get; set; }

        public override void Delete(ReleaseContext dbContext)
        {
            Titles?.Delete(dbContext);
            References?.Delete(dbContext);
            base.Delete(dbContext);
        }
    }
}
