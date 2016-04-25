using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Content
{
    /// <summary>
    /// A base class for all database entities that contain a localized text property, attributes and images
    /// </summary>
    public abstract class BaseWithImages : BaseWithAttributes
    {
        [ForeignKey("Images")]
        public int? ImagesId { get; set; }
        public virtual ImageSet Images { get; set; }

        [NotMapped]
        public Image Cover => Images?.Collection?.OrderBy(item => item.Order).FirstOrDefault();

        /// <summary>
        /// Adds new images to the model
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="images">A collection of images to add to the model</param>
        public void AddImages(ReleaseContext dbContext, List<Image> images)
        {
            if (images == null || images.Count == 0)
            {
                return;
            }

            if (Images == null)
            {
                Images = new ImageSet(images);
            }
            else
            {
                ((List<Image>)Images.Collection).AddRange(images);
            }

            Images.SortAndOrderSequential();
        }

        public void RemoveImage(ReleaseContext dbContext, int id)
        {
            throw new NotImplementedException("do you even need it?");//TODO: remove this method?
        }

        public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {
            ImageSet imageSet = ((BaseWithImages)sourceModel).Images;
            if (imageSet?.Collection?.Count > 0)
            {
                this.AddImages(dbContext, imageSet.Collection as List<Image>);
            }

            base.UpdateModel(dbContext, sourceModel);
        }

        public override void Delete(ReleaseContext dbContext)
        {
            base.Delete(dbContext);
            Images?.Delete(dbContext);
        }
    }
}
