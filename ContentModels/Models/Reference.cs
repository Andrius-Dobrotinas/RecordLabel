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
        [Display(ResourceType = typeof(ContentLocalization), Name = "Reference_DisplayAs")]
        public string DisplayAs {
            get
            {
                if (Type == ReferenceType.Youtube)
                {
                    return GenerateYoutubeLink();
                }
                else
                {
                    return Target?.Replace("http://", "").Replace("https://", "");
                }
            }
        }

        [Display(ResourceType = typeof(ContentLocalization), Name = "Order")]
        public int Order { get; set; }

        public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {
            base.UpdateModel(dbContext, sourceModel);

            Reference source = (Reference)sourceModel;
            Target = source.Target;
            Type = source.Type;
        }

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