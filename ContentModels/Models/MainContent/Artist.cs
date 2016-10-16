using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Data.Models
{
    [UsesGenre]
    public class Artist : MainContent
    {
        [Required]
        public string Name { get; set; }
    }
}