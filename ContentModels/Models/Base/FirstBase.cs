using System;
using System.ComponentModel.DataAnnotations;

namespace RecordLabel.Data.Models
{
    /// <summary>
    /// Database entity class with Id as a primary key
    /// </summary>
    public abstract class FirstBase : EntityBase
    {
        [Key]
        public override int Id { get; set; }
    }
}