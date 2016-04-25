using RecordLabel.Content.Localization;
using System.ComponentModel.DataAnnotations;

namespace RecordLabel.Content.Metadata
{
    public enum AttributeType
    {
        [Display(ResourceType = typeof(ContentLocalization), Name = "AttributeType_Attribute")]
        Attribute = 0,

        [Display(ResourceType = typeof(ContentLocalization), Name = "AttributeType_Genre")]
        Genre = 1
    }
}
