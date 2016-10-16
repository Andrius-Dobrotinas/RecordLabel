using System;
using System.Collections.Generic;

namespace RecordLabel.Data.Models
{
    public class Reference : ReferenceBase
    {
        public virtual IList<MainContent> Owners
        {
            get
            {
                return owners ?? (owners = new List<MainContent>());
            }
            set
            {
                owners = value;
            }
        }

        private IList<MainContent> owners;
    }
}