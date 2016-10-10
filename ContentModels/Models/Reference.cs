using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecordLabel.Content.Localization;

namespace RecordLabel.Content
{
    [OneToOneRelationship]
    public class Reference : Base, IKnowIfImEmpty, IValueComparable<Reference>
    {
        public static string YoutubeLinkBase { get; set; }

        [Required]
        [Display(ResourceType = typeof(ContentLocalization), Name = "Reference_Target")]
        public string Target { get; set; }

        [Required]
        [Display(ResourceType = typeof(ContentLocalization), Name = "Reference_Type")]
        public ReferenceType Type { get; set; }

        [NotMapped]
        public string TargetUrl
        {
            get
            {
                if (Type == ReferenceType.Youtube)
                {
                    return GenerateYoutubeLink();
                }
                else if ((Type == ReferenceType.Website || Type == ReferenceType.File) &&
                    !(Target.StartsWith("http") || Target.StartsWith("ftp")))
                {
                    return $"http://{Target}";
                }
                else
                {
                    return Target;
                }
            }
        }

        [NotMapped]
        [Display(ResourceType = typeof(ContentLocalization), Name = "Reference_DisplayTargetAs")]
        public string DisplayTargetAs => Target?.Replace("http://", String.Empty).Replace("https://", String.Empty).Replace("ftp://", String.Empty);

        [Display(ResourceType = typeof(ContentLocalization), Name = "Order")]
        public int Order { get; set; }

        public bool ValuesEqual(Reference reference)
        {
            return reference != null &&
                Target == reference.Target &&
                Type == reference.Type &&
                ClassHelper.CompareReferenceTypes(Localization, reference.Localization);
        }

        public virtual bool IsEmpty
        {
            get
            {
                return String.IsNullOrWhiteSpace(Target) &&
                    (Localization?.IsEmpty ?? true);
            }
        }

        private string GenerateYoutubeLink()
        {
            return $"{YoutubeLinkBase}{Target}";
        }
    }
}