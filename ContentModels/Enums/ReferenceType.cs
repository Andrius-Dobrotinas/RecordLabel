using System.ComponentModel.DataAnnotations;
using RecordLabel.Content.Localization;

namespace RecordLabel.Content
{
    public enum ReferenceType
    {
        [Display(Name = "ReferenceType_Website", ResourceType = typeof(ContentLocalization))]
        Website = 0,
        [Display(Name = "ReferenceType_File", ResourceType = typeof(ContentLocalization))]
        File = 1,
        [Display(Name = "ReferenceType_Youtube", ResourceType = typeof(ContentLocalization))]
        Youtube = 2,
        [Display(Name = "ReferenceType_OtherVideo", ResourceType = typeof(ContentLocalization))]
        OtherVideo = 3
    }
}
