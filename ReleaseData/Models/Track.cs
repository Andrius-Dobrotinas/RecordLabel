﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Catalogue
{
    [OneToOneRelationship]
    public class Track : FirstBase, IKnowIfImEmpty, IValueComparable<Track>
    {
        [Required]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Title")]
        public string Title { get; set; }

        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }
        [Display(ResourceType = typeof(ModelLocalization), Name = "Reference")]
        public virtual Reference Reference { get; set; }

        public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {
            Track source = (Track)sourceModel;
            Title = source.Title;
            if (Reference != null)
            {
                if (source.Reference != null)
                {
                    Reference.UpdateModel(dbContext, source.Reference);
                }
                else
                {
                    Reference.Delete(dbContext);
                }
            }
            else
            {
                Reference = source.Reference;
            }
        }

        public bool ValuesEqual(Track track)
        {
            return track != null &&
                Title == track.Title &&
                ModelHelpers.CompareReferenceTypes(Reference, track.Reference);
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
