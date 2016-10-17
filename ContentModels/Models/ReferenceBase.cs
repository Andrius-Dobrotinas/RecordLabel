using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Data.Models
{
    public abstract class ReferenceBase : FirstBase
    {
        /*[Key]
        public virtual int Id { get; set; }*/

        [Required]
        public string Target { get; set; }

        [Required]
        public ReferenceType Type { get; set; }

        public int Order { get; set; }
    }
}
