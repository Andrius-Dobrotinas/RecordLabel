using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Data.Models
{
    public abstract class LocalizedStringBase<TTarget> : LocalizedStringBase
    {
        [ForeignKey("TargetObject"), Key, Column(Order = 0)]
        public int TargetObjectId { get; set; }
        public virtual TTarget TargetObject { get; set; }
    }

    public abstract class LocalizedStringBase : EntityBase
    {
        [NotMapped]
        public override int Id { get; set; }

        [Key, Column(Order = 1)]
        public Language Language { get; set; }

        public string Text { get; set; }
    }
}
