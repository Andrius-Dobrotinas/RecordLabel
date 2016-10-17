using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Data.Models
{
    public abstract class ReferenceBase : FirstBase
    {
        [Required]
        public string Target { get; set; }

        [Required]
        public ReferenceType Type { get; set; }

        public int Order { get; set; }
    }
}
