using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Data.Models
{
    public class Article : MainContent
    {
        public DateTime Date { get; set; }
        
        public ArticleType Type { get; set; }
        
        public string Author { get; set; }
    }
}
