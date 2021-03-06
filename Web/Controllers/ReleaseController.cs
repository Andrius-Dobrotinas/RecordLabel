﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecordLabel.Content;
using System.Data.Entity;
using System.Linq.Expressions;

namespace RecordLabel.Web.Controllers
{
    public sealed class ReleaseController : BaseWithImagesController<Release>
    {
        protected override string ListViewTitle => Localization.ApplicationLocalization.List_Releases;

        public ReleaseController() : base(new ReleaseContext(), context => context.Releases)
        {
            ListModelQuery = initQuery => initQuery.Include(entity => entity.Artist); //init.Include(entity => entity.Images).Include(entity => entity.Artist);
            CompleteModelQuery = initQuery => ListModelQuery(initQuery).Include(entity => entity.References).Include(entity => entity.MasterVersion).Include(entity => entity.Descriptions).Include(entity => entity.Tracklist);
            IndexViewName = "~/Views/Shared/List.cshtml";
            GetItemHtmlForAjax = (model) => View("ExtensionPartials/ListItemWithImage", new ViewModels.ListItemWithImageModel { Model = model as BaseWithImages, AdminMode = Global.IsAdminMode });
        }

        protected override void OnModelValidation(Release postedModel)
        {
            base.OnModelValidation(postedModel);

            // TODO: validate MasterVersion (check if the Id corresponds to any release with IsMasterVersion == true
            // TODO: review and implement other validations

            //Make sure IsMasterVersion has a valid value
            if (postedModel.IsMasterVersion == true)
            {
                //If this is a slave to some other release (has a primary version specified)
                if (postedModel.MasterVersionId.HasValue && postedModel.MasterVersionId.Value > 0)
                {
                    postedModel.IsMasterVersion = false;
                }
            }
            else
            {
                //If this is a primary version for some other release (specified as a primary version on some other release)
                if (EntitySet.Any(item => item.MasterVersionId == postedModel.Id))
                {
                    postedModel.IsMasterVersion = true;
                }
            }
        }

        protected override void PrepareViewBagForCreate()
        {
            ViewBag.ArtistId = DbContext.Set<Artist>().ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Name);
            ViewBag.MediaId = DbContext.Set<Content.Metadata.MediaType>().ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Text);

            SelectListItem[] primaryVersions = EntitySet.Where(item => item.IsMasterVersion == true).ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => $"{item.Title} ({item.Media.Name}, {item.CatalogueNumber}, {item.Date})");
            if (primaryVersions.Length > 0)
            {
                primaryVersions = primaryVersions.NewListWithFirstDefaultItem("0", "None");
            }
            ViewBag.MasterVersionId = primaryVersions;

            base.PrepareViewBagForCreate();
        }

        protected override void PrepareViewBagForEdit(Release model)
        {
            ViewBag.ArtistId = DbContext.Set<Artist>().ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Name, item => model.ArtistId == item.Id);
            ViewBag.MediaId = DbContext.Set<Content.Metadata.MediaType>().ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => item.Text, item => model.MediaId == item.Id);
          
            //If this is a IsMasterVersion version and there are slave versions, don't load Other versions dropdown menu
            if ((model.IsMasterVersion && EntitySet.Any(item => item.MasterVersionId == model.Id)))// model.MasterVersion.Any(); ?
            {
                ViewBag.IsMasterVersionDisabled = true;
            }
            else
            {
                //Don't let change IsMasterVersion value if this is a slave version
                if (model.MasterVersionId != null)
                {
                    ViewBag.IsMasterVersionDisabled = true;
                }

                SelectListItem[] primaryVersions = EntitySet.Where(item => item.IsMasterVersion == true && item.Id != model.Id).ToArray().ToListOfSelectListItems(item => item.Id.ToString(), item => $"{item.Title} ({item.Media.Name}, {item.CatalogueNumber}, {item.Date})", item => item.Id == model.MasterVersionId).ToArray();
                if (primaryVersions.Length > 0)
                {
                    primaryVersions = primaryVersions.NewListWithFirstDefaultItem("0", "None");
                }
                ViewBag.MasterVersionId = primaryVersions;
            }

            base.PrepareViewBagForEdit(model);
        }

        protected override void PrepareViewBagForView(Release model)
        {
            //Load other version if this is a Master release
            model.LoadOtherVersions(EntitySet);

            base.PrepareViewBagForEdit(model);
        }

        protected override Release SelectModel(int id)
        {
            DbContext.Set<Content.Metadata.MediaType>().Load();
            return base.SelectModel(id);
        }

        protected override Release[] SelectModels(int batch)
        {
            return SelectModels(batch, null);
        }

        protected override Release[] SelectModels(int batch, Expression<Func<Release, bool>> filter)
        {
            Release[] releases = base.SelectModels(batch, filter);
            releases.ForEach(item => item.LoadOtherVersions(EntitySet));
            DbContext.Set<Content.Metadata.MediaType>().Load();
            return releases;
        }


        [AjaxOnly]
        public EmptyResult List(int batch)
        {
            return GetFilteredModelsPartial(batch, null);
        }

        public ActionResult ByArtist(int id)
        {
            return GetFilteredModels(0, item => item.ArtistId == id);
        }

        [AjaxOnly]
        public EmptyResult ByArtist(int id, int batch)
        {
            return GetFilteredModelsPartial(batch, item => item.ArtistId == id);
        }

        public ActionResult ByAttribute(int id)
        {
            return GetFilteredModels(0, item => item.Attributes.Collection.FirstOrDefault(i => i.Id == id) != null);
        }

        [AjaxOnly]
        public EmptyResult ByAttribute(int id, int batch)
        {
            return GetFilteredModelsPartial(batch, item => item.Attributes.Collection.FirstOrDefault(i => i.Id == id) != null);
        }
    }
}