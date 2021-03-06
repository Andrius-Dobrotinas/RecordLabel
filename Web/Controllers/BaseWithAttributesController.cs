﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RecordLabel.Content;
using RecordLabel.Content.Metadata;

namespace RecordLabel.Web.Controllers
{
    public abstract class BaseWithAttributesController<TModel> : EntityBaseController<TModel> where TModel : BaseWithAttributes
    {
        public BaseWithAttributesController(ReleaseContext dbContext, Func<ReleaseContext, DbSet<TModel>> entitySet) : base(dbContext, entitySet)
        {

        }

        private SelectListItem[] SelectAllAttributes()
        {
            //Currently, there are only two types of attributes
            if (typeof(TModel).GetCustomAttributes(typeof(UsesGenreAttribute), true).Any() == false)
            {
                return DbContext.Set<RecordLabel.Content.Metadata.Attribute>().Where(item => item.Type == AttributeType.Attribute).ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Name);
            }
            else
            {
                SelectListGroup attributeGroup = new SelectListGroup() { Name = "Attribures" };
                SelectListGroup genreGroup = new SelectListGroup() { Name = "Genres" };
                return DbContext.Set<RecordLabel.Content.Metadata.Attribute>().ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Name, item => (item.Type == AttributeType.Attribute) ? attributeGroup : genreGroup);
            }
        }

        private SelectListItem[] SelectAttributesForEdit(TModel model)
        {
            //Currently, there are only two types of attributes
            if (typeof(TModel).GetCustomAttributes(typeof(UsesGenreAttribute), true).Any() == false)
            {
                return DbContext.Set<RecordLabel.Content.Metadata.Attribute>().Where(item => item.Type == AttributeType.Attribute).ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Name, item => model?.Attributes?.Collection?.Contains(item) ?? false);
            }
            else
            {
                SelectListGroup attributeGroup = new SelectListGroup() { Name = "Attribures" };
                SelectListGroup genreGroup = new SelectListGroup() { Name = "Genres" };
                return DbContext.Set<RecordLabel.Content.Metadata.Attribute>().ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Name, item => model?.Attributes?.Collection?.Contains(item) ?? false, item => (item.Type == AttributeType.Attribute) ? attributeGroup : genreGroup);
            }
        }

        protected override void PrepareViewBagForCreate()
        {
            ViewBag.AttributeIds = SelectAllAttributes();
            base.PrepareViewBagForCreate();
        }
        protected override void PrepareViewBagForEdit(TModel model)
        {
            ViewBag.AttributeIds = SelectAttributesForEdit(model);
            base.PrepareViewBagForEdit(model);
        }
    }
}