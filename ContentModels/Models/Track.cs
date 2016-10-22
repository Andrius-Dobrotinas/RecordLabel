using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecordLabel.okay.Localization;

namespace RecordLabel.Data.Models
{
    public class Track : FirstBase
    {
        public int MainContentId { get; set; }
        public virtual MainContent MainContent { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual TrackReference Reference { get; set; }
    }
}
