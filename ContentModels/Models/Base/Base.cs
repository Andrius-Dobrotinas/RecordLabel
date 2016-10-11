using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RecordLabel.Content.Localization;

namespace RecordLabel.Content
{
    /// <summary>
    /// A base class for all database entities with a localized text property
    /// </summary>
    public abstract class Base : FirstBase, IHasASet<LocalString>
    {
        /// <summary>
        /// Main property for localized text
        /// </summary>
        [ForeignKey("Localization")]
        public int? LocalizationId { get; set; }
        public virtual LocalStringSet Localization { get; set; }

        /// <summary>
        /// Text from localization in current (or, if not available, default) language 
        /// </summary>
        [NotMapped]
        [Display(ResourceType = typeof(ContentLocalization), Name = "Base_Text")]
        public string Text => Localization?.Text;
        
        public override void Delete(ReleaseContext dbContext)
        {
            Localization?.Delete(dbContext);
            base.Delete(dbContext);
        }
    }
}
