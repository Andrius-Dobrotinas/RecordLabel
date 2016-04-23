using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace RecordLabel.Catalogue
{
    /// <summary>
    /// Database entity class with Id as a primary key
    /// </summary>
    public abstract class FirstBase : Entity
    {
        [Key]
        public override int Id { get; set; }
    }
}