using System.ComponentModel.DataAnnotations;
using RecordLabel.Catalogue;

namespace RecordLabel
{
    public enum ArticleType
    {
        [Display(Name = "ArticleType_News", ResourceType = typeof(ModelLocalization))]
        News = 0,
        [Display(Name = "ArticleType_Article", ResourceType = typeof(ModelLocalization))]
        Article = 1
    }
}
