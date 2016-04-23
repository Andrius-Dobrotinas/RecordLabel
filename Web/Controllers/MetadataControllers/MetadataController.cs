using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RecordLabel.Catalogue;
namespace RecordLabel.Web.Controllers
{
    public abstract class MetadataController<TModel> : EntityBaseController<TModel> where TModel : Catalogue.Metadata.Metadata<TModel>
    {
        public MetadataController(ReleaseContext dbContext, Func<ReleaseContext, DbSet<TModel>> entitySet) : base(dbContext, entitySet)
        {
            ItemsPerPage = int.MaxValue;
        }

        public override ActionResult View(int id)
        {
            return RedirectToAction("Edit", new { Id = id });
        }

        protected override void PrepareViewBagForCreate()
        {
            ViewBag.ItemType = typeof(TModel).Name;
        }
    }
}
