﻿using System.ComponentModel.DataAnnotations;
using RecordLabel.okay.Localization;

namespace RecordLabel.Data.Models
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
