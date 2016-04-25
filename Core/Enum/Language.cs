using System.ComponentModel.DataAnnotations;

namespace RecordLabel
{
    public enum Language
    {
        [LanugageCode("en")]
        [Display(ResourceType = typeof(LocalizationResource), Name = "Enum_English")]
        English = 0,

        [LanugageCode("lt")]
        [Display(ResourceType = typeof(LocalizationResource), Name = "Enum_Lithuanian")]
        Lithuanian = 1
    }
}