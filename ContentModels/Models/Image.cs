using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.IO;
using System;

namespace RecordLabel.Content
{
    public class Image : FirstBase, IValueComparable<Image>
    {
        /// <summary>
        /// Name of a file on the file system
        /// </summary>
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// Name for displaying
        /// </summary>
        [Required]
        public string Name { get; set; }

        public ImageType Type { get; set; }

        /// <summary>
        /// An position in which the image must be displayed when in a set of images
        /// </summary>
        public int Order { get; set; }

        public override void Delete(ReleaseContext dbContext)
        {
            base.Delete(dbContext);
        }

        // TODO
        /*public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {
            Image model = (Image)sourceModel;
            Order = model.Order;
            Name = model.Name;
            Type = model.Type;
            FileName = model.FileName;
        }*/

        //Not using this yet
        bool IValueComparable<Image>.ValuesEqual(Image compareTo)
        {
            throw new NotImplementedException();
        }
    }
}
