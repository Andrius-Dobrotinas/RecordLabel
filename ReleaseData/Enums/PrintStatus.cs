using System.ComponentModel.DataAnnotations;

namespace RecordLabel.Catalogue
{
    public enum PrintStatus
    {
        [Display(Name = "PrintStatus_InPrint", ResourceType = typeof(ModelLocalization))]
        InPrint = 0,
        [Display(Name = "PrintStatus_OutOfPrint", ResourceType = typeof(ModelLocalization))]
        OutOfPrint = 1
    }
}