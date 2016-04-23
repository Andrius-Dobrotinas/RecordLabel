using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using schema = System.ComponentModel.DataAnnotations.Schema;
using RecordLabel.Catalogue;

namespace RecordLabel.Catalogue.Metadata
{
    /// <summary>
    /// Base class for simple shared reusable (One-To-Many type relationship) entities that have a mandatory localized text property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Metadata<T> : Base, IKnowIfImEmpty
    {
        /// <summary>
        /// Localized name
        /// </summary>
        [schema.NotMapped]
        [Display(ResourceType = typeof(ModelLocalization), Name = "Metadata_Name")]
        public string Name => base.Text;

        [Required]
        public override LocalStringSet Localization
        {
            get
            {
                return base.Localization;
            }

            set
            {
                base.Localization = value;
            }
        }

        public virtual bool IsEmpty
        {
            get
            {
                return Localization?.IsEmpty ?? true;
            }
        }
    }
}