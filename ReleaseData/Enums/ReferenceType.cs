using System.ComponentModel.DataAnnotations;
using RecordLabel.Catalogue;

namespace RecordLabel
{
    public enum ReferenceType
    {
        [Display(Name = "ReferenceType_Website", ResourceType = typeof(ModelLocalization))]
        Website = 0,
        [Display(Name = "ReferenceType_File", ResourceType = typeof(ModelLocalization))]
        File = 1,
        [Display(Name = "ReferenceType_Youtube", ResourceType = typeof(ModelLocalization))]
        Youtube = 2,
        [Display(Name = "ReferenceType_OtherVideo", ResourceType = typeof(ModelLocalization))]
        OtherVideo = 3
    }
}
