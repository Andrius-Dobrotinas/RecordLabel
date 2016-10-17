using System;
using System.Collections.Generic;

namespace RecordLabel.Data.Models
{
    public class Reference : ReferenceBase
    {
        public MainContent Owner { get; set; }

        private IList<MainContent> owners;
    }
}