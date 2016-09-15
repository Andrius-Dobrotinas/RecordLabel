using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using RecordLabel.Content;

namespace RecordLabel.Web.Controllers
{
    public class ArticlesController : BaseWithAttributesController<Article>
    {
        protected override string ListViewTitle => Localization.ApplicationLocalization.List_Articles;

        public ArticlesController() : base(new ReleaseContext(), context => context.Articles)
        {
            ListModelQuery = initQuery => initQuery.Include(entity => entity.Titles);
            CompleteModelQuery = initQuery => ListModelQuery(initQuery).Include(entity => entity.Attributes).Include(entity => entity.References);
            IndexViewName = "~/Views/Shared/List.cshtml";
        }

        protected override Article SelectModel(int id)
        {
            var news = base.SelectModels(0);
            ViewData.Add("News", news);
            return base.SelectModel(id);
        }
    }
}