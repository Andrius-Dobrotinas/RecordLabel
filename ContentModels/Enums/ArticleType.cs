using System.ComponentModel.DataAnnotations;
using RecordLabel.okay.Localization;

namespace RecordLabel.Data.Models
{
    public enum ArticleType
    {
        [Display(Name = "ArticleType_News", ResourceType = typeof(ContentLocalization))]
        News = 0,
        [Display(Name = "ArticleType_Article", ResourceType = typeof(ContentLocalization))]
        Article = 1
    }
}
