using RecordLabel.Data.Localization;
using System.ComponentModel.DataAnnotations;

namespace RecordLabel.Data.Models
{
    public enum MetadataType
    {
        [Display(ResourceType = typeof(ContentLocalization), Name = "AttributeType_Attribute")]
        Attribute = 0,

        [Display(ResourceType = typeof(ContentLocalization), Name = "AttributeType_Genre")]
        Genre = 1
    }
}
