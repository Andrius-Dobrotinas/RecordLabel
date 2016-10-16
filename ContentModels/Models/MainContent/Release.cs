using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Data.Models
{
    [UsesGenre]
    public class Release : MainContent
    {
        [Required]
        public string Title { get; set; }
        
        [ForeignKey("Artist"), Required]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        
        public string Subtitle { get; set; }
        
        public short? Date { get; set; }
        
        public short? DateRecorded { get; set; }
        
        public short? Length { get; set; }
        
        public string MusicBy { get; set; }
        
        public string LyricsBy { get; set; }

        [Required]
        public string CatalogueNumber { get; set; }

        [ForeignKey("Media"), Required]
        public int MediaId { get; set; }
        public MediaType Media { get; set; }

        [Required]
        public PrintStatus PrintStatus { get; set; } = PrintStatus.InPrint;
    }
}
