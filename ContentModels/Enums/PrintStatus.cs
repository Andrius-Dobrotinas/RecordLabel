using System.ComponentModel.DataAnnotations;
using RecordLabel.Content.Localization;

namespace RecordLabel.Content
{
    public enum PrintStatus
    {
        [Display(Name = "PrintStatus_InPrint", ResourceType = typeof(ContentLocalization))]
        InPrint = 0,
        [Display(Name = "PrintStatus_OutOfPrint", ResourceType = typeof(ContentLocalization))]
        OutOfPrint = 1
    }
}