using System.ComponentModel.DataAnnotations;
using RecordLabel.Content.Localization;

namespace RecordLabel.Content
{
    public enum ArticleType
    {
        [Display(Name = "ArticleType_News", ResourceType = typeof(ContentLocalization))]
        News = 0,
        [Display(Name = "ArticleType_Article", ResourceType = typeof(ContentLocalization))]
        Article = 1
    }
}
