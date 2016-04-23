using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using RecordLabel.Catalogue;
using System.Reflection;
using System.Linq.Expressions;
using System.IO;

namespace RecordLabel.Web.Controllers
{
    public abstract class BaseWithImagesController<TModel> : BaseWithAttributesController<TModel> where TModel : BaseWithImages
    {
        public BaseWithImagesController(ReleaseContext dbContext, Func<ReleaseContext, DbSet<TModel>> entitySet) : base(dbContext, entitySet)
        {
            ListModelQuery = initQuery => initQuery.Include(entity => entity.Images); //.Collection ?
        }

        protected override void OnModelDelete(TModel model)
        {
            IList<Tuple<string, ImageType>> files = null;
            if (model.Images?.Collection.Count > 0)
            {
                files = model.Images.Collection.Select(image => new Tuple<string, ImageType>(image.FileName, image.Type)).ToArray();
            }

            base.OnModelDelete(model);

            if (files != null)
            {
                foreach (var file in files)
                {
                    try {
                        ImageHelper.DeleteImagePhysically(file.Item1, file.Item2);
                    }
                    catch
                    {
                        continue;
                        //TODO: inform about the error
                        //TODO: log this exception
                    }
                }
            }
        }
    }
}