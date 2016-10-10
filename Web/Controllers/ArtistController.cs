using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecordLabel.Content;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using RecordLabel.Web.ModelBinding;

namespace RecordLabel.Web.Controllers
{
    public class ArtistController : BaseWithImagesController<Artist>
    {
        public ArtistController() : base(new ReleaseContext(), context => context.Artists)
        {
            ItemsPerPage = int.MaxValue;
            IndexViewName = "~/Views/Shared/List.cshtml";
        }

        protected override void PrepareViewBagForCreate()
        {
            ViewBag.ReleaseIds = DbContext.Set<Release>().ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Title);
            base.PrepareViewBagForCreate();
        }

        protected override void PrepareViewBagForEdit(Artist model)
        {
            ViewBag.ReleaseIds = DbContext.Set<Release>().ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Title, item => model?.Releases?.Contains(item) ?? false);
            base.PrepareViewBagForEdit(model);
        }
    }
}