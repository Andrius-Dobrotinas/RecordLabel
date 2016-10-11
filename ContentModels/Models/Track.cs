using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecordLabel.Content.Localization;

namespace RecordLabel.Content
{
    [OneToOneRelationship]
    public class Track : FirstBase, IKnowIfImEmpty, IValueComparable<Track>
    {
        [Required]
        [Display(ResourceType = typeof(ContentLocalization), Name = "Title")]
        public string Title { get; set; }

        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }
        [Display(ResourceType = typeof(ContentLocalization), Name = "Reference")]
        public virtual Reference Reference { get; set; }

        public bool ValuesEqual(Track track)
        {
            return track != null &&
                Title == track.Title &&
                ClassHelper.CompareReferenceTypes(Reference, track.Reference);
        }

        bool IKnowIfImEmpty.IsEmpty
        {
            get
            {
                return String.IsNullOrEmpty(Title) &&
                    (Reference?.IsEmpty ?? true);
            }
        }

        public override void Delete(ReleaseContext dbContext)
        {
            Reference?.Delete(dbContext);
            base.Delete(dbContext);
        }
    }
}
