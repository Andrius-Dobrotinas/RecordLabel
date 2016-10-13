using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecordLabel.Content.Localization;

namespace RecordLabel.Content
{
    public class TrackReference : ReferenceBase
    {
        public int TrackId { get; set; }
        public Track Track { get; set; }

        /*public override bool ValuesEqual(TrackReference reference)
        {
            return base.ValuesEqual(reference);
        }*/
    }

    public class Reference : ReferenceBase
    {
        [ForeignKey("Set")]
        public int SetId { get; set; }
        public ReferenceSet Set { get; set; }
    }

    [OneToOneRelationship]
    public abstract class ReferenceBase : Base, IKnowIfImEmpty, IValueComparable<ReferenceBase>
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

        public virtual bool ValuesEqual(ReferenceBase reference)
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