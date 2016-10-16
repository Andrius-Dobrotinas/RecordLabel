using System;
using System.Collections.Generic;

namespace RecordLabel.Data.Models
{
    /// <summary>
    /// Base class for all database entities with a localized text property
    /// </summary>
    public abstract class Base<TLocalizedString> : FirstBase, IHasASet<TLocalizedString>
        where TLocalizedString : LocalizedStringBase
    {
        public virtual IList<TLocalizedString> LocalizedText
        {
            get
            {
                return collection ?? (collection = new List<TLocalizedString>());
            }
            set
            {
                collection = value;
            }
        }
        private IList<TLocalizedString> collection { get; set; }
    }
}
