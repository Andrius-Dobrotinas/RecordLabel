using System.ComponentModel.DataAnnotations;

namespace RecordLabel
{
    public enum Language
    {
        [LanugageCode("en")]
        [Display(ResourceType = typeof(LocalizationResource), Name = "Enum_English")]
        English = 0,

        [LanugageCode("jp")]
        [Display(ResourceType = typeof(LocalizationResource), Name = "Enum_Japanese")]
        Japanese = 1
    }
}